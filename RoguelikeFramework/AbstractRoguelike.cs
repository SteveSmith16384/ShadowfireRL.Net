using RLNET;
using RoguelikeFramework.components;
using RoguelikeFramework.models;
using RoguelikeFramework.systems;
using RoguelikeFramework.view;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace RoguelikeFramework {

    public abstract class AbstractRoguelike : IDataForView {

        public enum InputMode { Normal, SelectItemFromInv, SelectItemFromFloor, SelectMapPoint, ActivatingCurrentItem };
        public enum InputSubMode { None, ThrowingItem, DroppingItem, PickingUpItem, SelectingDestination };

        protected BasicEcs ecs;
        protected MapData mapData;
        protected GameLog gameLog;
        private string hoverText;
        private List<Point> line;
        private InputMode currentInputMode = InputMode.Normal;
        private InputSubMode currentInputSubMode = InputSubMode.None;
        protected DefaultRLView view;

        // Systems
        private DrawingSystem drawingSystem;
        protected CheckMapVisibilitySystem checkVisibilitySystem;
        private PickupDropSystem pickupItemSystem;
        private EffectsSystem effectsSystem;
        private ThrowingSystem throwingSystem;
        private ExplosionSystem explosionSystem;

        protected AbstractEntity currentUnit;
        public Dictionary<int, AbstractEntity> playersUnits = new Dictionary<int, AbstractEntity>();

        protected Dictionary<int, AbstractEntity> menuItemList = new Dictionary<int, AbstractEntity>(); // For when selecting item to pick up etc...

        public AbstractRoguelike(int maxLogEntries) {
            this.ecs = new BasicEcs();
            this.mapData = new MapData();
            this.gameLog = new GameLog(maxLogEntries);

            this.CreateData();

            this.view = new DefaultRLView(this);
            this.drawingSystem = new DrawingSystem(this.view, this, true); // todo - setting

            this.checkVisibilitySystem = new CheckMapVisibilitySystem(this.mapData);
            this.ecs.systems.Add(new ShootOnSightSystem(this.checkVisibilitySystem, this.ecs.entities));

            this.checkVisibilitySystem.process(this.playersUnits.Values);
            this.ecs.systems.Add(new MovementSystem(this.mapData, this.checkVisibilitySystem));
            this.explosionSystem = new ExplosionSystem(this.checkVisibilitySystem, this.mapData);
            this.ecs.systems.Add(new TimerCountdownSystem(this.explosionSystem));
            this.pickupItemSystem = new PickupDropSystem();
            this.effectsSystem = new EffectsSystem();
            this.throwingSystem = new ThrowingSystem(this.mapData);

            // Draw screen
            this.drawingSystem.process(this.effectsSystem.effects);
        }


        protected abstract void CreateData();


        public void HandleKeyInput(RLKeyPress keyPress) {
            if (this.effectsSystem.HasEffects()) {
                return;
            }

            bool action_performed = false; // todo - remove this?

            switch (keyPress.Key) {

                case RLKey.Number1:
                case RLKey.Number2:
                case RLKey.Number3:
                case RLKey.Number4:
                case RLKey.Number5:
                case RLKey.Number6:
                case RLKey.Number7:
                case RLKey.Number8:
                case RLKey.Number9:
                    int idx = keyPress.Key - RLKey.Number0;
                    if (this.currentInputMode == InputMode.SelectItemFromFloor) {
                        if (this.currentInputSubMode == InputSubMode.PickingUpItem) {
                            PositionComponent pos = (PositionComponent)this.currentUnit.GetComponent(nameof(PositionComponent));
                            this.pickupItemSystem.PickupItem(this.currentUnit, this.menuItemList[idx], this.mapData.map[pos.x, pos.y]);
                            this.gameLog.Add("item picked up");
                            this.currentInputMode = InputMode.Normal;
                            this.menuItemList.Clear();
                            action_performed = true;
                        }
                    } else if (this.currentInputMode == InputMode.SelectItemFromInv) {
                        if (this.currentInputSubMode == InputSubMode.DroppingItem) {
                            PositionComponent pos = (PositionComponent)this.currentUnit.GetComponent(nameof(PositionComponent));
                            this.pickupItemSystem.DropItem(this.currentUnit, this.menuItemList[idx], this.mapData.map[pos.x, pos.y]);
                            this.gameLog.Add("item dropped up");
                            this.currentInputMode = InputMode.Normal;
                            this.menuItemList.Clear();
                            action_performed = true;
                        }
                    } else if (this.currentInputMode == InputMode.ActivatingCurrentItem) {
                        TimerCanBeSetComponent tcbsc = (TimerCanBeSetComponent)this.currentUnit.GetComponent(nameof(TimerCanBeSetComponent));
                        if (tcbsc.activated == false) {
                            tcbsc.activated = true;
                            tcbsc.timeLeft = idx;
                            this.gameLog.Add("Timer set for " + idx);
                            action_performed = true;
                        } else {
                            this.gameLog.Add("Timer already set!");
                        }
                    } else {
                        this.SelectUnit(idx);
                    }
                    break;

                case RLKey.Up: {
                        MovementDataComponent m = (MovementDataComponent)this.currentUnit.GetComponent(nameof(MovementDataComponent));
                        m.dest = null;
                        m.offY = -1;
                        action_performed = true;
                        break;
                    }
                case RLKey.Down: {
                        MovementDataComponent m = (MovementDataComponent)this.currentUnit.GetComponent(nameof(MovementDataComponent));
                        m.dest = null;
                        m.offY = 1;
                        action_performed = true;
                        break;
                    }
                case RLKey.Left: {
                        MovementDataComponent m = (MovementDataComponent)this.currentUnit.GetComponent(nameof(MovementDataComponent));
                        m.dest = null;
                        m.offX = -1;
                        action_performed = true;
                        break;
                    }
                case RLKey.Right: {
                        MovementDataComponent m = (MovementDataComponent)this.currentUnit.GetComponent(nameof(MovementDataComponent));
                        m.dest = null;
                        m.offX = 1;
                        action_performed = true;
                        break;
                    }

                case RLKey.Space: {
                        MobDataComponent mdc = (MobDataComponent)this.currentUnit.GetComponent(nameof(MobDataComponent));
                        if (mdc.actionPoints > 0) {
                            mdc.actionPoints -= 100;
                        }
                        action_performed = true;
                        break;
                    }

                case RLKey.A: // Activate
                    this.currentInputMode = InputMode.ActivatingCurrentItem;
                    this.gameLog.Add("Enter number from 1-9");
                    break;

                case RLKey.D: { // Drop
                        this.currentInputMode = InputMode.SelectItemFromInv;
                        this.currentInputSubMode = InputSubMode.DroppingItem;
                        this.ListItemsInInv();
                        this.gameLog.Add("Please select an item to drop");
                        break;
                    }

                case RLKey.P: { // Pickup
                        this.currentInputMode = InputMode.SelectItemFromFloor;
                        this.currentInputSubMode = InputSubMode.PickingUpItem;
                        this.ListItemsOnFloor();
                        this.gameLog.Add("Please select an item to pick-up");
                        break;
                    }

                case RLKey.S: // Stop moving
                    var md = (MovementDataComponent)this.currentUnit.GetComponent(nameof(MobDataComponent));
                    md.dest = null;
                    this.gameLog.Add("Movement stopped");
                    break;

                case RLKey.T: { // Throw
                        this.currentInputMode = InputMode.SelectMapPoint;
                        this.currentInputSubMode = InputSubMode.ThrowingItem;
                        this.gameLog.Add("Select where to throw");
                        break;
                    }

                case RLKey.W: { // Walk
                        this.currentInputMode = InputMode.SelectMapPoint;
                        this.currentInputSubMode = InputSubMode.SelectingDestination;
                        this.gameLog.Add("Select where to walk to");
                        break;
                    }
            }

            //if (action_performed) {
            this.SingleGameLoop();
            //}
            this.checkVisibilitySystem.process(this.playersUnits.Values);
        }


        protected void SelectUnit(int num) {
            if (this.playersUnits.ContainsKey(num)) {
                this.currentUnit = this.playersUnits[num];
                this.gameLog.Add(this.currentUnit.name + " selected");
            }
        }


        private void SingleGameLoop() {
            this.ecs.process(); // To move the player's units
            //while (true) {
            // Check at least one player's entity has > 100 APs
            foreach (var unit in this.playersUnits.Values) {
                MobDataComponent mdc = (MobDataComponent)unit.GetComponent(nameof(MobDataComponent));
                if (mdc.actionPoints > 0) {
                    // todo - select unit if current unit has no APs left
                    return; // They still have spare APs, so don't do anything and wait for the player
                }
            }
            while (true) {
                foreach (var e in this.ecs.entities) {
                    if (this.playersUnits.ContainsValue(e) == false) { // Don't check player's units
                        MobDataComponent mdc = (MobDataComponent)e.GetComponent(nameof(MobDataComponent));
                        if (mdc != null) {
                            if (mdc.actionPoints > 0) {// They still have spare APs
                                Console.WriteLine($"{e.name} still has APs");
                                this.ecs.process();
                                // todo - sleep for a sec so the player can see what's going on
                                continue;
                            }
                        }
                    }
                }
                break;
            }

            // Give everyone some APs
            foreach (var e in this.ecs.entities) {
                MobDataComponent mdc = (MobDataComponent)e.GetComponent(nameof(MobDataComponent));
                if (mdc != null) {
                    mdc.actionPoints += 100;
                }
            }
            //}
        }


        public void HandleMouseEvent(RLMouse mouse) {
            if (this.effectsSystem.HasEffects()) {
                return;
            }

            if (mouse.LeftPressed) {
                bool action_performed = false;
                if (this.currentInputMode == InputMode.SelectMapPoint) {
                    if (this.currentInputSubMode == InputSubMode.SelectingDestination) {
                        var pos = (PositionComponent)this.currentUnit.GetComponent(nameof(PositionComponent));
                        var md = (MovementDataComponent)this.currentUnit.GetComponent(nameof(MovementDataComponent));
                        md.dest = Misc.GetLine(pos.x, pos.y, mouse.X, mouse.Y);
                    } else if (this.currentInputSubMode == InputSubMode.ThrowingItem) {
                        this.throwingSystem.ThrowItem(this.currentUnit, mouse.X, mouse.Y);
                        action_performed = true;
                    }
                }

                if (action_performed) {
                    this.SingleGameLoop();
                }
                //this.checkVisibilitySystem.process(this.playersUnits.Values);

            } else {
                this.hoverText = this.GetSquareDesc(mouse.X, mouse.Y);
                if (this.currentInputMode == InputMode.SelectMapPoint) {
                    var pos = (PositionComponent)this.currentUnit.GetComponent(nameof(PositionComponent));
                    this.line = Misc.GetLine(pos.x, pos.y, mouse.X, mouse.Y);
                } else {
                    this.line = null;
                }
            }
        }


        private string GetSquareDesc(int x, int y) {
            if (x < this.mapData.map.GetLength(0) && y < this.mapData.map.GetLength(1)) {
                var sb = new StringBuilder();
                var ents = this.mapData.map[x, y];
                //return string.Join(", ", ents);
                foreach (var e in ents) {
                    sb.Append(e.name + "; ");
                }

                return sb.ToString();
            }
            return "";
        }


        public MapData GetMapData() {
            return this.mapData;
        }


        public string GetHoverText() {
            return this.hoverText;
        }


        public List<string> GetLog() {
            return this.gameLog.GetEntries();
        }


        public List<Point> GetLine() {
            return this.line;
        }

        public Dictionary<int, AbstractEntity> GetUnits() {
            return this.playersUnits;
        }


        public AbstractEntity GetCurrentUnit() {
            return this.currentUnit;
        }


        public void Repaint() {
            this.effectsSystem.process();
            this.drawingSystem.process(this.effectsSystem.effects);
        }


        public List<string> GetStatsFor(AbstractEntity e) {
            return this.GetStatsFor_Sub(e);
        }


        protected abstract List<string> GetStatsFor_Sub(AbstractEntity e);


        public Dictionary<int, AbstractEntity> GetItemSelectionList() {
            return this.menuItemList;
        }


        private void ListItemsOnFloor() {
            this.menuItemList.Clear();
            int idx = 1;
            PositionComponent pos = (PositionComponent)this.currentUnit.GetComponent(nameof(PositionComponent));
            List<AbstractEntity> items = this.mapData.map[pos.x, pos.y];
            foreach (var e in items) {
                CarryableComponent cc = (CarryableComponent)e.GetComponent(nameof(CarryableComponent));
                if (cc != null) {
                    this.menuItemList.Add(idx, e);
                    idx++;
                }
            }
        }


        private void ListItemsInInv() {
            this.menuItemList.Clear();
            int idx = 1;
            CanCarryComponent ccc = (CanCarryComponent)this.currentUnit.GetComponent(nameof(CanCarryComponent));
            foreach (var e in ccc.GetItems()) {
                CarryableComponent cc = (CarryableComponent)e.GetComponent(nameof(CarryableComponent));
                if (cc != null) {
                    this.menuItemList.Add(idx, e);
                    idx++;
                }
            }
        }

    }
}
