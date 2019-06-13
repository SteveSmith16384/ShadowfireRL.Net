using RLNET;
using RoguelikeFramework.components;
using RoguelikeFramework.models;
using RoguelikeFramework.systems;
using RoguelikeFramework.view;
using ShadowfireRL.effects;
using SimpleEcs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;

namespace RoguelikeFramework {

    public abstract class AbstractRoguelike : IEcsEventListener, IDataForView, IDebugSettings {

        public enum InputMode { Normal, SelectItemFromInv, SelectItemFromFloor, SelectMapPoint, ActivatingCurrentItem };
        public enum InputSubMode { None, ThrowingItem, DroppingItem, PickingUpItem, SelectingDestination, SelectShotTarget };

        protected BasicEcs ecs;
        protected MapData mapData;
        protected GameLog gameLog;
        private string hoverText;
        private List<Point> line;
        private InputMode currentInputMode = InputMode.Normal;
        private InputSubMode currentInputSubMode = InputSubMode.None;
        protected DefaultRLView view;
        private bool doRepaint = true;

        // Systems
        private DrawingSystem drawingSystem;
        protected CheckMapVisibilitySystem checkVisibilitySystem;
        private PickupDropSystem pickupItemSystem;
        private EffectsSystem effectsSystem;
        private ExplosionSystem explosionSystem;
        private DamageSystem damageSystem;

        protected AbstractEntity currentUnit;
        public List<AbstractEntity> playersUnits = new List<AbstractEntity>();

        protected Dictionary<int, AbstractEntity> menuItemList = new Dictionary<int, AbstractEntity>(); // For when selecting item to pick up etc...

        public AbstractRoguelike(int maxLogEntries) {
            this.ecs = new BasicEcs(this);
            this.mapData = new MapData();
            this.gameLog = new GameLog(maxLogEntries);

            new CloseCombatSystem(this.ecs);
            new MovementSystem(this.ecs, this.mapData);

            this.CreateData();

            this.view = new DefaultRLView(this);
            this.drawingSystem = new DrawingSystem(this.view, this);

            this.checkVisibilitySystem = new CheckMapVisibilitySystem(this.ecs, this.mapData);
            new ShootOnSightSystem(this.ecs, this.checkVisibilitySystem, this.ecs.entities);

            this.checkVisibilitySystem.process(this.playersUnits);
            this.damageSystem = new DamageSystem(this.ecs, this.gameLog);
            this.explosionSystem = new ExplosionSystem(this.ecs, this.checkVisibilitySystem, this.damageSystem, this.mapData, this.ecs.entities);
            new TimerCountdownSystem(this.ecs, this.explosionSystem);
            this.pickupItemSystem = new PickupDropSystem();
            this.effectsSystem = new EffectsSystem(this.ecs);
            new ThrowingSystem(this.ecs, this.mapData, this.gameLog);
            new ShootingSystem(this.ecs, this.gameLog);

            // Draw screen
            this.drawingSystem.Process(this.effectsSystem.effects);
        }


        protected abstract void CreateData();


        public void HandleKeyInput(RLKeyPress keyPress) {
            this.doRepaint = true;

            if (this.effectsSystem.HasEffects()) {
                return;
            }

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
                        }
                    } else if (this.currentInputMode == InputMode.SelectItemFromInv) {
                        if (this.currentInputSubMode == InputSubMode.DroppingItem) {
                            PositionComponent pos = (PositionComponent)this.currentUnit.GetComponent(nameof(PositionComponent));
                            this.pickupItemSystem.DropItem(this.currentUnit, this.menuItemList[idx], this.mapData.map[pos.x, pos.y]);
                            this.gameLog.Add("item dropped up");
                            this.currentInputMode = InputMode.Normal;
                            this.menuItemList.Clear();
                        }
                    } else if (this.currentInputMode == InputMode.ActivatingCurrentItem) {
                        TimerCanBeSetComponent tcbsc = (TimerCanBeSetComponent)this.currentUnit.GetComponent(nameof(TimerCanBeSetComponent));
                        if (tcbsc != null) {
                            if (tcbsc.activated == false) {
                                tcbsc.activated = true;
                                tcbsc.timeLeft = idx;
                                this.gameLog.Add("Timer set for " + idx);
                            } else {
                                this.gameLog.Add("Timer already set!");
                            }
                        }
                    } else {
                        this.SelectUnit(idx);
                    }
                    break;

                case RLKey.Up: {
                        MovementDataComponent m = (MovementDataComponent)this.currentUnit.GetComponent(nameof(MovementDataComponent));
                        m.route = null;
                        m.offY = -1;
                        break;
                    }
                case RLKey.Down: {
                        MovementDataComponent m = (MovementDataComponent)this.currentUnit.GetComponent(nameof(MovementDataComponent));
                        m.route = null;
                        m.offY = 1;
                        break;
                    }
                case RLKey.Left: {
                        MovementDataComponent m = (MovementDataComponent)this.currentUnit.GetComponent(nameof(MovementDataComponent));
                        m.route = null;
                        m.offX = -1;
                        break;
                    }
                case RLKey.Right: {
                        MovementDataComponent m = (MovementDataComponent)this.currentUnit.GetComponent(nameof(MovementDataComponent));
                        m.route = null;
                        m.offX = 1;
                        break;
                    }

                case RLKey.Space: {
                        MobDataComponent mdc = (MobDataComponent)this.currentUnit.GetComponent(nameof(MobDataComponent));
                        if (mdc.actionPoints > 0) {
                            mdc.actionPoints -= 100;
                        }
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
                    var md = (MovementDataComponent)this.currentUnit.GetComponent(nameof(MovementDataComponent));
                    md.route = null;
                    this.gameLog.Add("Movement stopped");
                    break;

                case RLKey.T: { // Throw
                        this.currentInputMode = InputMode.SelectMapPoint;
                        this.currentInputSubMode = InputSubMode.ThrowingItem;
                        this.gameLog.Add("Select where to throw");
                        break;
                    }

                case RLKey.U: { // Use (e.g. shoot)
                        this.UseCurrentItem();
                        break;
                    }

                case RLKey.W: { // Walk
                        this.currentInputMode = InputMode.SelectMapPoint;
                        this.currentInputSubMode = InputSubMode.SelectingDestination;
                        this.gameLog.Add("Select where to walk to");
                        break;
                    }

                case RLKey.X: { // Test
                        BulletEffect be = new BulletEffect(0, 0, 20, 20);
                        EffectsSystem es = (EffectsSystem)this.ecs.GetSystem(nameof(EffectsSystem));
                        es.effects.Add(be);
                        break;
                    }
            }

            //if (action_performed) {
            this.SingleGameLoop();
            //}
            this.checkVisibilitySystem.process(this.playersUnits);
        }


        public void HandleMouseEvent(RLMouse mouse) {
            this.doRepaint = true;

            if (this.effectsSystem.HasEffects()) {
                return;
            }

            if (mouse.LeftPressed) {
                bool action_performed = false;
                if (this.currentInputMode == InputMode.SelectMapPoint) {
                    if (this.currentInputSubMode == InputSubMode.SelectingDestination) {
                        var pos = (PositionComponent)this.currentUnit.GetComponent(nameof(PositionComponent));
                        var md = (MovementDataComponent)this.currentUnit.GetComponent(nameof(MovementDataComponent));
                        md.route = Misc.GetLine(pos.x, pos.y, mouse.X, mouse.Y, true);
                        this.gameLog.Add("Destination selected");
                        this.currentInputMode = InputMode.Normal;
                    } else if (this.currentInputSubMode == InputSubMode.ThrowingItem) {
                        var throwingSystem = (ThrowingSystem)this.ecs.GetSystem(nameof(ThrowingSystem));
                        throwingSystem.ThrowItem(this.currentUnit, mouse.X, mouse.Y);
                        this.currentInputMode = InputMode.Normal;
                        action_performed = true;
                    } else if (this.currentInputSubMode == InputSubMode.SelectShotTarget) {
                        ShootingSystem ss = (ShootingSystem)this.ecs.GetSystem(nameof(ShootingSystem));
                        MobDataComponent att = (MobDataComponent)this.currentUnit.GetComponent(nameof(MobDataComponent));
                        AbstractEntity target = ss.GetTargetAt(this.mapData.map[mouse.X, mouse.Y], mouse.X, mouse.Y, att.side);
                        var pos = (PositionComponent)this.currentUnit.GetComponent(nameof(PositionComponent));
                        ss.EntityShootingAtEntity(this.currentUnit, new Point(pos.x, pos.y), target, new Point(mouse.X, mouse.Y));
                    }
                } else if (this.currentInputMode == InputMode.Normal) {
                    // Have they clicked on an adjacent square
                    var pos = (PositionComponent)this.currentUnit.GetComponent(nameof(PositionComponent));
                    if (GeometryFunctions.Distance(mouse.X, mouse.Y, pos.x, pos.y) < 2) {
                        this.MouseClicked(mouse.X, mouse.Y);
                    }

                }

                if (action_performed) {
                    this.SingleGameLoop();
                }

            } else {
                this.hoverText = this.GetSquareDesc(mouse.X, mouse.Y);

                if (this.currentInputMode == InputMode.SelectMapPoint) {
                    var pos = (PositionComponent)this.currentUnit.GetComponent(nameof(PositionComponent));
                    this.line = Misc.GetLine(pos.x, pos.y, mouse.X, mouse.Y, true);
                } else {
                    this.line = null;
                }
            }
        }


        protected virtual void MouseClicked(int x, int y) {

        }


        protected void SelectUnit(int num) {
            if (num <= this.playersUnits.Count) {
                if (this.currentUnit != this.playersUnits[num - 1]) {
                    this.currentUnit = this.playersUnits[num - 1];
                    this.gameLog.Add(this.currentUnit.name + " selected");
                }
            }
        }


        private void SingleGameLoop() {
            this.doRepaint = true;
            this.ecs.process(); // To move the player's units

            // Check at least one player's entity has > 100 APs
            foreach (var unit in this.playersUnits) {
                MobDataComponent mdc = (MobDataComponent)unit.GetComponent(nameof(MobDataComponent));
                if (mdc.actionPoints > 0) {
                    MovementDataComponent moveData = (MovementDataComponent)unit.GetComponent(nameof(MovementDataComponent));
                    if (moveData.route == null || moveData.route.Count == 0) {
                        return; // They still have spare APs and no dest, so don't do anything and wait for the player
                    }
                }
            }

            // Loop around all entities until none have any 
            bool keepLooping = true;
            while (keepLooping) {
                keepLooping = false;
                foreach (var e in this.ecs.entities) {
                    if (this.playersUnits.Contains(e) == false) { // Don't check player's units
                        MobDataComponent mdc = (MobDataComponent)e.GetComponent(nameof(MobDataComponent));
                        if (mdc != null) {
                            if (mdc.actionPoints > 0) {// They still have spare APs
                                Console.WriteLine($"{e.name} still has APs");
                                this.ecs.process();
                                this.doRepaint = true;
                                Thread.Sleep(1000);//  sleep for a sec so the player can see what's going on
                                keepLooping = true;
                            }
                        }
                    }
                }
            }

            // Give everyone some APs
            foreach (var e in this.ecs.entities) {
                MobDataComponent mdc = (MobDataComponent)e.GetComponent(nameof(MobDataComponent));
                if (mdc != null) {
                    mdc.actionPoints += mdc.apsPerTurn;
                    if (e.name == "Alien") {
                        Console.WriteLine($"Alien now has {mdc.actionPoints} APs left");
                    }

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

        public List<AbstractEntity> GetUnits() {
            return this.playersUnits;
        }


        public AbstractEntity GetCurrentUnit() {
            return this.currentUnit;
        }


        public void Repaint() {
            //if (this.doRepaint) { // Caused flickering?
            this.doRepaint = false;
            this.effectsSystem.Process();
            this.drawingSystem.Process(this.effectsSystem.effects);
            //}
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


        private void UseCurrentItem() {
            ItemCanShootComponent icsc = (ItemCanShootComponent)this.currentUnit.GetComponent(nameof(ItemCanShootComponent));
            if (icsc != null) {
                this.currentInputMode = InputMode.SelectMapPoint;
                this.currentInputSubMode = InputSubMode.SelectShotTarget;
            }
        }


        public void EntityRemoved(AbstractEntity entity) {
            if (entity == this.currentUnit) {
                this.playersUnits.Remove(entity);
                if (this.playersUnits.Count > 0) {
                    this.currentUnit = this.playersUnits[0];
                } else {
                    // todo - game over!
                }
            }
        }

    }

}