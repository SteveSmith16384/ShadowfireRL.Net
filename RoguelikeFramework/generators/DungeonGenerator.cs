using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace RogueLikeMapBuilder {

    // Code taken from http://www.evilscience.co.uk/roguelike-dungeon-generator-using-c/ and then I fixed the bugs.
    public class csMapbuilder {

        public int[,] map;

        /// <summary>
        /// Built rooms stored here
        /// </summary>
        private List<Rectangle> rctBuiltRooms;

        /// <summary>
        /// Built corridors stored here
        /// </summary>
        private List<Point> lBuilltCorridors;

        /// <summary>
        /// Corridor to be built stored here
        /// </summary>
        private List<Point> lPotentialCorridor;

        /// <summary>
        /// Room to be built stored here
        /// </summary>
        private Rectangle rctCurrentRoom;


        //room properties
        public Size Room_Min_Size;
        public Size Room_Max_Size { get; set; }
        public int MaxRooms { get; set; }
        public int MinRoomDistance { get; set; }
        public int MinCorridorDistance { get; set; }

        //corridor properties
        public int MinCorridorLength { get; set; }
        //[Category("Corridor"), Description("Maximum corridor length"), DisplayName("Maximum length")]
        public int MaxCorridorLength { get; set; }
        //[Category("Corridor"), Description("Maximum turns"), DisplayName("Maximum turns")]
        public int Corridor_MaxTurns { get; set; }
        //[Category("Corridor"), Description("The distance a corridor has to be away from a closed cell for it to be built"), DisplayName("Corridor Spacing")]
        public int CorridorSpace;


        [Category("Probability"), Description("Probability of building a corridor from a room or corridor. Greater than value = room"), DisplayName("Select room")]
        public int BuildProb { get; set; }

        public Size Map_Size { get; set; }
        public int BreakOut;

        /// <summary>
        /// describes the outcome of the corridor building operation
        /// </summary>
        enum CorridorItemHit {
            invalid //invalid point generated
            ,
            self  //corridor hit self
                ,
            existingcorridor //hit a built corridor
                ,
            originroom //corridor hit origin room 
                ,
            existingroom //corridor hit existing room
                ,
            completed //corridor built without problem    
                ,
            tooclose
                , OK //point OK
        }

        Point[] directions_straight = new Point[]{
                                            new Point(0, -1) //n
                                            , new Point(0, 1)//s
                                            , new Point(1, 0)//w
                                            , new Point(-1, 0)//e
                                        };

        private int filledcell = 1;
        private int emptycell = 0;
        private int door = 2;

        Random rnd = new Random();

        public csMapbuilder(int x, int y) {
            this.Map_Size = new Size(x, y);
            this.map = new int[this.Map_Size.Width, this.Map_Size.Height];
            this.Corridor_MaxTurns = 5;
            this.Room_Min_Size = new Size(3, 3);
            this.Room_Max_Size = new Size(15, 15);
            this.MinCorridorLength = 5;
            this.MaxCorridorLength = 15;
            this.MaxRooms = 5;

            this.MinRoomDistance = 5;
            this.MinCorridorDistance = 5;
            this.CorridorSpace = 3;

            this.BuildProb = 50;
            this.BreakOut = 250;
        }

        /// <summary>
        /// Initialise everything
        /// </summary>
        private void Clear() {
            this.lPotentialCorridor = new List<Point>();
            this.rctBuiltRooms = new List<Rectangle>();
            this.lBuilltCorridors = new List<Point>();

            this.map = new int[this.Map_Size.Width, this.Map_Size.Height];
            for (int x = 0; x < this.Map_Size.Width; x++)
                for (int y = 0; y < this.Map_Size.Width; y++)
                    this.map[x, y] = this.filledcell;
        }

        #region build methods()

        /// <summary>
        /// Randomly choose a room and attempt to build a corridor terminated by
        /// a room off it, and repeat until MaxRooms has been reached. The map
        /// is started of by placing a room in approximately the centre of the map
        /// using the method PlaceStartRoom()
        /// </summary>
        /// <returns>Bool indicating if the map was built, i.e. the property BreakOut was not
        /// exceed</returns>
        public bool Build_OneStartRoom() {
            CorridorItemHit CorBuildOutcome;
            Point Location = new Point();
            Point Direction = new Point();

            this.Clear();

            this.PlaceStartRoom();

            //attempt to build the required number of rooms
            int loopctr = 0;
            while (this.rctBuiltRooms.Count() < this.MaxRooms) {
                if (loopctr++ > this.BreakOut) { // bail out if this value is exceeded
                    return false;
                }

                if (this.Corridor_GetStart(out Location, out Direction)) {
                    CorBuildOutcome = this.CorridorMake_Straight(ref Location, ref Direction, this.rnd.Next(1, this.Corridor_MaxTurns), this.rnd.Next(0, 100) > 50 ? true : false);

                    switch (CorBuildOutcome) {
                        case CorridorItemHit.existingroom:
                        case CorridorItemHit.existingcorridor:
                        case CorridorItemHit.self:
                            this.Corridor_Build();
                            break;

                        case CorridorItemHit.completed:
                            if (this.Room_AttemptBuildOnCorridor(Direction)) {
                                this.Corridor_Build();
                                this.Room_Build();
                            }
                            break;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Randomly choose a room and attempt to build a corridor terminated by
        /// a room off it, and repeat until MaxRooms has been reached. The map
        /// is started of by placing two rooms on opposite sides of the map and joins
        /// them with a long corridor, using the method PlaceStartRooms()
        /// </summary>
        /// <returns>Bool indicating if the map was built, i.e. the property BreakOut was not
        /// exceed</returns>
        public bool Build_ConnectedStartRooms() {
            int loopctr = 0;

            CorridorItemHit CorBuildOutcome;
            Point Location = new Point();
            Point Direction = new Point();

            this.Clear();

            this.PlaceStartRooms();

            //attempt to build the required number of rooms
            while (this.rctBuiltRooms.Count() < this.MaxRooms) {

                if (loopctr++ > this.BreakOut)//bail out if this value is exceeded
                    return false;

                if (this.Corridor_GetStart(out Location, out Direction)) {

                    CorBuildOutcome = this.CorridorMake_Straight(ref Location, ref Direction, this.rnd.Next(1, this.Corridor_MaxTurns), this.rnd.Next(0, 100) > 50 ? true : false);

                    switch (CorBuildOutcome) {
                        case CorridorItemHit.existingroom:
                        case CorridorItemHit.existingcorridor:
                        case CorridorItemHit.self:
                            this.Corridor_Build();
                            break;

                        case CorridorItemHit.completed:
                            if (this.Room_AttemptBuildOnCorridor(Direction)) {
                                this.Corridor_Build();
                                this.Room_Build();
                            }
                            break;
                    }
                }
            }

            return true;

        }

        #endregion


        #region room utilities

        /// <summary>
        /// Place a random sized room in the middle of the map
        /// </summary>
        private void PlaceStartRoom() {
            this.rctCurrentRoom = new Rectangle() {
                Width = this.rnd.Next(this.Room_Min_Size.Width, this.Room_Max_Size.Width),
                Height = this.rnd.Next(this.Room_Min_Size.Height, this.Room_Max_Size.Height)
            };
            this.rctCurrentRoom.X = this.Map_Size.Width / 2;
            this.rctCurrentRoom.Y = this.Map_Size.Height / 2;
            this.Room_Build();
        }


        /// <summary>
        /// Place a start room anywhere on the map
        /// </summary>
        private void PlaceStartRooms() {
            Point startdirection;
            bool connection = false;
            Point Location = new Point();
            Point Direction = new Point();
            CorridorItemHit CorBuildOutcome;

            while (!connection) {

                this.Clear();
                startdirection = this.Direction_Get(new Point());

                //place a room on the top and bottom
                if (startdirection.X == 0) {

                    //room at the top of the map
                    this.rctCurrentRoom = new Rectangle() {
                        Width = this.rnd.Next(this.Room_Min_Size.Width, this.Room_Max_Size.Width)
                                ,
                        Height = this.rnd.Next(this.Room_Min_Size.Height, this.Room_Max_Size.Height)
                    };
                    this.rctCurrentRoom.X = this.rnd.Next(0, this.Map_Size.Width - this.rctCurrentRoom.Width);
                    this.rctCurrentRoom.Y = 1;
                    this.Room_Build();

                    //at the bottom of the map
                    this.rctCurrentRoom = new Rectangle();
                    this.rctCurrentRoom.Width = this.rnd.Next(this.Room_Min_Size.Width, this.Room_Max_Size.Width);
                    this.rctCurrentRoom.Height = this.rnd.Next(this.Room_Min_Size.Height, this.Room_Max_Size.Height);
                    this.rctCurrentRoom.X = this.rnd.Next(0, this.Map_Size.Width - this.rctCurrentRoom.Width);
                    this.rctCurrentRoom.Y = this.Map_Size.Height - this.rctCurrentRoom.Height - 1;
                    this.Room_Build();


                } else//place a room on the east and west side
                  {
                    //west side of room
                    this.rctCurrentRoom = new Rectangle();
                    this.rctCurrentRoom.Width = this.rnd.Next(this.Room_Min_Size.Width, this.Room_Max_Size.Width);
                    this.rctCurrentRoom.Height = this.rnd.Next(this.Room_Min_Size.Height, this.Room_Max_Size.Height);
                    this.rctCurrentRoom.Y = this.rnd.Next(0, this.Map_Size.Height - this.rctCurrentRoom.Height);
                    this.rctCurrentRoom.X = 1;
                    this.Room_Build();

                    this.rctCurrentRoom = new Rectangle();
                    this.rctCurrentRoom.Width = this.rnd.Next(this.Room_Min_Size.Width, this.Room_Max_Size.Width);
                    this.rctCurrentRoom.Height = this.rnd.Next(this.Room_Min_Size.Height, this.Room_Max_Size.Height);
                    this.rctCurrentRoom.Y = this.rnd.Next(0, this.Map_Size.Height - this.rctCurrentRoom.Height);
                    this.rctCurrentRoom.X = this.Map_Size.Width - this.rctCurrentRoom.Width - 2;
                    this.Room_Build();

                }



                if (this.Corridor_GetStart(out Location, out Direction)) {



                    CorBuildOutcome = this.CorridorMake_Straight(ref Location, ref Direction, 100, true);

                    switch (CorBuildOutcome) {
                        case CorridorItemHit.existingroom:
                            this.Corridor_Build();
                            connection = true;
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Make a room off the last point of Corridor, using
        /// CorridorDirection as an indicator of how to offset the room.
        /// The potential room is stored in Room.
        /// </summary>
        private bool Room_AttemptBuildOnCorridor(Point pDirection) {
            this.rctCurrentRoom = new Rectangle() {
                Width = this.rnd.Next(this.Room_Min_Size.Width, this.Room_Max_Size.Width)
                    ,
                Height = this.rnd.Next(this.Room_Min_Size.Height, this.Room_Max_Size.Height)
            };

            //startbuilding room from this point
            Point lc = this.lPotentialCorridor.Last();

            if (pDirection.X == 0) //north/south direction
            {
                this.rctCurrentRoom.X = this.rnd.Next(lc.X - this.rctCurrentRoom.Width + 1, lc.X);

                if (pDirection.Y == 1)
                    this.rctCurrentRoom.Y = lc.Y + 1;//south
                else
                    this.rctCurrentRoom.Y = lc.Y - this.rctCurrentRoom.Height - 1;//north


            } else if (pDirection.Y == 0)//east / west direction
              {
                this.rctCurrentRoom.Y = this.rnd.Next(lc.Y - this.rctCurrentRoom.Height + 1, lc.Y);

                if (pDirection.X == -1)//west
                    this.rctCurrentRoom.X = lc.X - this.rctCurrentRoom.Width;
                else
                    this.rctCurrentRoom.X = lc.X + 1;//east
            }

            return this.Room_Verify();
        }


        /// <summary>
        /// Randomly get a point on the edge of a randomly selected room
        /// </summary>
        /// <param name="Location">Out: Location of point on room edge</param>
        /// <param name="Location">Out: Direction of point</param>
        /// <returns>If Location is legal</returns>
        private void Room_GetEdge(out Point pLocation, out Point pDirection) {

            this.rctCurrentRoom = this.rctBuiltRooms[this.rnd.Next(0, this.rctBuiltRooms.Count())];

            //pick a random point within a room
            //the +1 / -1 on the values are to stop a corner from being chosen
            pLocation = new Point(this.rnd.Next(this.rctCurrentRoom.Left + 1, this.rctCurrentRoom.Right - 1)
                                  , this.rnd.Next(this.rctCurrentRoom.Top + 1, this.rctCurrentRoom.Bottom - 1));


            //get a random direction
            pDirection = this.directions_straight[this.rnd.Next(0, this.directions_straight.GetLength(0))];

            do {
                //move in that direction
                pLocation.Offset(pDirection);

                if (!this.Point_Valid(pLocation.X, pLocation.Y))
                    return;

                //until we meet an empty cell
            } while (this.Point_Get(pLocation.X, pLocation.Y) != this.filledcell);

        }

        #endregion

        #region corridor utitlies

        /// <summary>
        /// Randomly get a point on an existing corridor
        /// </summary>
        /// <param name="Location">Out: location of point</param>
        /// <returns>Bool indicating success</returns>
        private void Corridor_GetEdge(out Point pLocation, out Point pDirection) {
            List<Point> validdirections = new List<Point>();

            do {
                //the modifiers below prevent the first of last point being chosen
                pLocation = this.lBuilltCorridors[this.rnd.Next(1, this.lBuilltCorridors.Count - 1)];

                //attempt to locate all the empy map points around the location
                //using the directions to offset the randomly chosen point
                foreach (Point p in this.directions_straight)
                    if (this.Point_Valid(pLocation.X + p.X, pLocation.Y + p.Y))
                        if (this.Point_Get(pLocation.X + p.X, pLocation.Y + p.Y) == this.filledcell)
                            validdirections.Add(p);


            } while (validdirections.Count == 0);

            pDirection = validdirections[this.rnd.Next(0, validdirections.Count)];
            pLocation.Offset(pDirection);

        }

        /// <summary>
        /// Build the contents of lPotentialCorridor, adding it's points to the builtCorridors
        /// list then empty
        /// </summary>
        private void Corridor_Build() {
            foreach (Point p in this.lPotentialCorridor) {
                this.Point_Set(p.X, p.Y, this.emptycell);
                this.lBuilltCorridors.Add(p);
            }
            {
                Point p = this.lPotentialCorridor[0];
                this.Point_Set(p.X, p.Y, this.door);
                p = this.lPotentialCorridor.Last();
                this.Point_Set(p.X, p.Y, this.door);
            }
            this.lPotentialCorridor.Clear();
        }

        /// <summary>
        /// Get a starting point for a corridor, randomly choosing between a room and a corridor.
        /// </summary>
        /// <param name="Location">Out: pLocation of point</param>
        /// <param name="Location">Out: pDirection of point</param>
        /// <returns>Bool indicating if location found is OK</returns>
        private bool Corridor_GetStart(out Point pLocation, out Point pDirection) {
            this.rctCurrentRoom = new Rectangle();
            this.lPotentialCorridor = new List<Point>();

            if (this.lBuilltCorridors.Count > 0) {
                if (this.rnd.Next(0, 100) >= this.BuildProb)
                    this.Room_GetEdge(out pLocation, out pDirection);
                else
                    this.Corridor_GetEdge(out pLocation, out pDirection);
            } else//no corridors present, so build off a room
                this.Room_GetEdge(out pLocation, out pDirection);

            //finally check the point we've found
            return this.Corridor_PointTest(pLocation, pDirection) == CorridorItemHit.OK;

        }

        /// <summary>
        /// Attempt to make a corridor, storing it in the lPotentialCorridor list
        /// </summary>
        /// <param name="pStart">Start point of corridor</param>
        /// <param name="pTurns">Number of turns to make</param>
        private CorridorItemHit CorridorMake_Straight(ref Point pStart, ref Point pDirection, int pTurns, bool pPreventBackTracking) {

            this.lPotentialCorridor = new List<Point>();
            this.lPotentialCorridor.Add(pStart);

            int corridorlength;
            Point startdirection = new Point(pDirection.X, pDirection.Y);
            CorridorItemHit outcome;

            while (pTurns > 0) {
                pTurns--;

                corridorlength = this.rnd.Next(this.MinCorridorLength, this.MaxCorridorLength);
                //build corridor
                while (corridorlength > 0) {
                    corridorlength--;

                    //make a point and offset it
                    pStart.Offset(pDirection);

                    outcome = this.Corridor_PointTest(pStart, pDirection);
                    if (outcome != CorridorItemHit.OK)
                        return outcome;
                    else
                        this.lPotentialCorridor.Add(pStart);
                }

                if (pTurns > 1)
                    if (!pPreventBackTracking)
                        pDirection = this.Direction_Get(pDirection);
                    else
                        pDirection = this.Direction_Get(pDirection, startdirection);
            }

            return CorridorItemHit.completed;
        }

        /// <summary>
        /// Test the provided point to see if it has empty cells on either side
        /// of it. This is to stop corridors being built adjacent to a room.
        /// </summary>
        /// <param name="pPoint">Point to test</param>
        /// <param name="pDirection">Direction it is moving in</param>
        /// <returns></returns>
        private CorridorItemHit Corridor_PointTest(Point pPoint, Point pDirection) {

            if (!this.Point_Valid(pPoint.X, pPoint.Y))//invalid point hit, exit
                return CorridorItemHit.invalid;
            else if (this.lBuilltCorridors.Contains(pPoint))//in an existing corridor
                return CorridorItemHit.existingcorridor;
            else if (this.lPotentialCorridor.Contains(pPoint))//hit self
                return CorridorItemHit.self;
            else if (this.rctCurrentRoom != null && this.rctCurrentRoom.Contains(pPoint))//the corridors origin room has been reached, exit
                return CorridorItemHit.originroom;
            else {
                //is point in a room
                foreach (Rectangle r in this.rctBuiltRooms)
                    if (r.Contains(pPoint))
                        return CorridorItemHit.existingroom;
            }


            //using the property corridor space, check that number of cells on
            //either side of the point are empty
            foreach (int r in Enumerable.Range(-this.CorridorSpace, 2 * this.CorridorSpace + 1).ToList()) {
                if (pDirection.X == 0)//north or south
                {
                    if (this.Point_Valid(pPoint.X + r, pPoint.Y))
                        if (this.Point_Get(pPoint.X + r, pPoint.Y) != this.filledcell)
                            return CorridorItemHit.tooclose;
                } else if (pDirection.Y == 0)//east west
                  {
                    if (this.Point_Valid(pPoint.X, pPoint.Y + r))
                        if (this.Point_Get(pPoint.X, pPoint.Y + r) != this.filledcell)
                            return CorridorItemHit.tooclose;
                }

            }

            return CorridorItemHit.OK;
        }


        #endregion

        #region direction methods

        /// <summary>
        /// Get a random direction, excluding the opposite of the provided direction to
        /// prevent a corridor going back on it's Build
        /// </summary>
        /// <param name="dir">Current direction</param>
        /// <returns></returns>
        private Point Direction_Get(Point pDir) {
            Point NewDir;
            do {
                NewDir = this.directions_straight[this.rnd.Next(0, this.directions_straight.GetLength(0))];
            } while (this.Direction_Reverse(NewDir) == pDir);

            return NewDir;
        }

        /// <summary>
        /// Get a random direction, excluding the provided directions and the opposite of 
        /// the provided direction to prevent a corridor going back on it's self.
        /// 
        /// The parameter pDirExclude is the first direction chosen for a corridor, and
        /// to prevent it from being used will prevent a corridor from going back on 
        /// it'self
        /// </summary>
        /// <param name="dir">Current direction</param>
        /// <param name="pDirectionList">Direction to exclude</param>
        /// <param name="pDirExclude">Direction to exclude</param>
        /// <returns></returns>
        private Point Direction_Get(Point pDir, Point pDirExclude) {
            Point NewDir;
            do {
                NewDir = this.directions_straight[this.rnd.Next(0, this.directions_straight.GetLength(0))];
            } while (
                        this.Direction_Reverse(NewDir) == pDir
                         | this.Direction_Reverse(NewDir) == pDirExclude
                    );


            return NewDir;
        }

        private Point Direction_Reverse(Point pDir) {
            return new Point(-pDir.X, -pDir.Y);
        }

        #endregion

        #region room test

        /// <summary>
        /// Check if rctCurrentRoom can be built
        /// </summary>
        /// <returns>Bool indicating success</returns>
        private bool Room_Verify() {
            //make it one bigger to ensure that testing gives it a border
            this.rctCurrentRoom.Inflate(this.MinRoomDistance, this.MinRoomDistance);

            //check it occupies legal, empty coordinates
            for (int x = this.rctCurrentRoom.Left; x <= this.rctCurrentRoom.Right; x++)
                for (int y = this.rctCurrentRoom.Top; y <= this.rctCurrentRoom.Bottom; y++)
                    if (!this.Point_Valid(x, y) || this.Point_Get(x, y) != this.filledcell)
                        return false;

            //check it doesn't encroach onto existing rooms
            foreach (Rectangle r in this.rctBuiltRooms)
                if (r.IntersectsWith(this.rctCurrentRoom))
                    return false;

            this.rctCurrentRoom.Inflate(-this.MinRoomDistance, -this.MinRoomDistance);

            //check the room is the specified distance away from corridors
            this.rctCurrentRoom.Inflate(this.MinCorridorDistance, this.MinCorridorDistance);

            foreach (Point p in this.lBuilltCorridors)
                if (this.rctCurrentRoom.Contains(p))
                    return false;

            this.rctCurrentRoom.Inflate(-this.MinCorridorDistance, -this.MinCorridorDistance);

            return true;
        }

        /// <summary>
        /// Add the global Room to the rooms collection and draw it on the map
        /// </summary>
        private void Room_Build() {
            this.rctBuiltRooms.Add(this.rctCurrentRoom);

            for (int x = this.rctCurrentRoom.Left; x <= this.rctCurrentRoom.Right; x++)
                for (int y = this.rctCurrentRoom.Top; y <= this.rctCurrentRoom.Bottom; y++)
                    this.map[x, y] = this.emptycell;

        }

        #endregion

        #region Map Utilities

        /// <summary>
        /// Check if the point falls within the map array range
        /// </summary>
        /// <param name="x">x to test</param>
        /// <param name="y">y to test</param>
        /// <returns>Is point with map array?</returns>
        private Boolean Point_Valid(int x, int y) {
            return x >= 0 & x < this.map.GetLength(0) & y >= 0 & y < this.map.GetLength(1);
        }

        /// <summary>
        /// Set array point to specified value
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="val"></param>
        private void Point_Set(int x, int y, int val) {
            this.map[x, y] = val;
        }

        /// <summary>
        /// Get the value of the specified point
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private int Point_Get(int x, int y) {
            return this.map[x, y];
        }

        #endregion

        public delegate void moveDelegate();
        public event moveDelegate playerMoved;

    }

    /*
    public class Rectangle {

        public int X, Y, Width, Height;

        public int Left {
            get {
                return this.X;
            }
        }

        public int Right {
            get {
                return this.X + this.Width ;
            }
        }


        public int Top {
            get {
                return this.Y;
            }
        }

        public int Bottom {
            get {
                return this.Y + this.Height;
            }
        }


        public void Inflate(int ox, int oy) {
            this.X -= ox;
            this.Width += ox * 2;

            this.Y -= oy;
            this.Height += oy * 2;
        }


        public bool IntersectsWith(Rectangle b) {
            return this.Left < b.Right && b.Left < this.Right
                    && this.Top > b.Bottom && b.Top > this.Bottom; // Switch these if the origin is top-left, not bottom-left!
        }


        public bool Contains(int x, int y) {
            return this.Left < this.Right && this.Top > this.Bottom  // check for empty first
                    && x >= this.Left && x < this.Right && y <= this.Top && y > this.Bottom;
        }


        public bool Contains(Point p) {
            return this.Contains(p.x, p.y);
        }

    }*/
}
