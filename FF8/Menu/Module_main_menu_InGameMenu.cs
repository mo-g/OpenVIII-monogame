﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FF8
{
    internal partial class Module_main_menu_debug
    {
        private class IGM : Menu
        {
            #region Fields

            private Items choSideBar;
            private Matrix focus;
            private Mode mode;
            private Vector2 Size;
            private int _choChar;

            public int choChar
            {
                get
                {
                    if (_choChar >= 0 && _choChar < Data[SectionName.Party].Count)
                    {
                        if (Data[SectionName.Party].BLANKS[_choChar])
                            return choCharSet(_choChar + 1);
                    }
                    else if (_choChar < Data[SectionName.Non_Party].Count + Data[SectionName.Party].Count && _choChar >= Data[SectionName.Non_Party].Count)
                    {
                        if (Data[SectionName.Non_Party].BLANKS[_choChar - Data[SectionName.Party].Count])
                            return choCharSet(_choChar + 1);
                    }
                    return _choChar;
                }

                set => choCharSet(value);
            }

            private int choCharSet(int value)
            {
                while (true)
                {
                    if (value >= 0 && value < Data[SectionName.Party].Count)
                    {
                        if (Data[SectionName.Party].BLANKS[value])
                        {
                            if (_choChar > value) value--;
                            else if (_choChar < value) value++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else if (value < Data[SectionName.Non_Party].Count + Data[SectionName.Party].Count && value >= Data[SectionName.Party].Count)
                    {
                        if (Data[SectionName.Non_Party].BLANKS[value - Data[SectionName.Party].Count])
                        {
                            if (_choChar > value) value--;
                            else if (_choChar < value) value++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (value < 0)
                    {
                        value = Data[SectionName.Non_Party].Count + Data[SectionName.Party].Count - 1;
                        _choChar = int.MaxValue;
                    }
                    else if (value >= Data[SectionName.Non_Party].Count + Data[SectionName.Party].Count)
                    {
                        value = 0;
                        _choChar = int.MinValue;
                    }
                }

                _choChar = value;
                return value;
            }

            #endregion Fields

            #region Enums

            public enum Items
            {
                Junction,
                Item,
                Magic,
                Status,
                GF,
                Ability,
                Switch,
                Card,
                Config,
                Tutorial,
                Save
            }

            private enum Mode
            {
                ChooseItem,
                ChooseChar,
                Junction,
                Item,
                Magic,
                Status,
                GF,
                Ability,
                Switch,
                Card,
                Config,
                Tutorial,
                Save
            }

            #endregion Enums

            #region Methods

            public override void Draw()
            {
                Memory.SpriteBatchStartAlpha(ss: SamplerState.PointClamp, tm: focus);

                switch (mode)
                {
                    case Mode.ChooseChar:
                    case Mode.ChooseItem:
                    default:
                        foreach (var i in Data)
                            i.Value.Draw();
                        break;
                }
                switch (mode)
                {
                    case Mode.ChooseChar:
                        DrawPointer(Data[SectionName.SideMenu].CURSOR[(int)choSideBar], blink: true);

                        if (choChar < Data[SectionName.Party].Count && choChar >= 0)
                            DrawPointer(Data[SectionName.Party].CURSOR[choChar]);
                        else if (choChar < Data[SectionName.Non_Party].Count + Data[SectionName.Party].Count && choChar >= Data[SectionName.Party].Count)
                            DrawPointer(Data[SectionName.Non_Party].CURSOR[choChar - Data[SectionName.Party].Count]);

                        break;

                    default:
                        DrawPointer(Data[SectionName.SideMenu].CURSOR[(int)choSideBar]);
                        break;
                }
                Memory.SpriteBatchEnd();
            }



            public enum SectionName
            {
                Header,
                Footer,
                SideMenu,
                Clock,
                Party,
                Non_Party
            }

            protected override void Init()
            {
                Size = new Vector2 { X = 843, Y = 630 };
                TextScale = new Vector2(2.545455f, 3.0375f);
                IGMData Header = new IGMData_Header(0, 0, new IGMDataItem_Box(pos: new Rectangle { Width = 610, Height = 75 }, title: Icons.ID.HELP));
                IGMData Footer = new IGMData_Footer(0, 0, new IGMDataItem_Box(pos: new Rectangle { Width = 610, Height = 75, Y = 630 - 75 }));
                IGMData Clock = new IGMData_Clock(1, 8, new IGMDataItem_Box(pos: new Rectangle { Width = 226, Height = 114, Y = 630 - 114, X = 843 - 226 }));
                IGMData SideMenu = new IGMData_SideMenu(11, 1, new IGMDataItem_Box(pos: new Rectangle { Width = 226, Height = 492, X = 843 - 226 }));
                IGMData Non_Party = new IGMData_NonParty(6, 7, new IGMDataItem_Box(pos: new Rectangle { Width = 580, Height = 231, X = 20, Y = 318 }));
                IGMData Party = new IGMData_Party(3, 7, new IGMDataItem_Empty(pos: new Rectangle { Width = 580, Height = 234, X = 20, Y = 84 }));
                Data.Add(SectionName.Header, Header);
                Data.Add(SectionName.Footer, Footer);
                Data.Add(SectionName.Clock, Clock);
                Data.Add(SectionName.SideMenu, SideMenu);
                Data.Add(SectionName.Party, Party);
                Data.Add(SectionName.Non_Party, Non_Party);
                Update();
            }


            

            private void Update_PartyStatus_Box(sbyte pos, Saves.Characters character)
            {
                if (Data[SectionName.Non_Party].SIZE != null)
                {
                    if (character != Saves.Characters.Blank)
                    {
                        float yoff = 6;
                        int num = 0;
                        int spaces = 0;

                        Data[SectionName.Party].ITEM[pos, 0] = new IGMDataItem_Box(Memory.Strings.GetName((Faces.ID)character), title: Icons.ID.STATUS);
                        Tuple<Rectangle, Point, Rectangle> dims = DrawBox(Data[SectionName.Party].SIZE[pos], ((IGMDataItem_Box)Data[SectionName.Party].ITEM[pos, 0]).Data, indent: false, skipdraw: true);
                        Rectangle r = dims.Item3;
                        Data[SectionName.Party].ITEM[pos, 0].Pos = dims.Item1;
                        Data[SectionName.Party].CURSOR[pos] = dims.Item2;

                        r = dims.Item3;
                        r.Offset(184, yoff);
                        Data[SectionName.Party].ITEM[pos, 1] = new IGMDataItem_Icon(Icons.ID.Lv, r, 13);

                        r = dims.Item3;
                        num = Memory.State.Characters[(int)character].Level;
                        spaces = 3 - num.ToString().Length;
                        r.Offset((229 + spaces * 20), yoff);
                        Data[SectionName.Party].ITEM[pos, 2] = new IGMDataItem_Int(num, r, 2, 0, 1);

                        r = dims.Item3;
                        r.Offset(304, yoff);
                        Data[SectionName.Party].ITEM[pos, 3] = new IGMDataItem_Icon(Icons.ID.HP2, r, 13);

                        r = dims.Item3;
                        num = Memory.State.Characters[(int)character].CurrentHP;
                        spaces = 4 - num.ToString().Length;
                        r.Offset((354 + spaces * 20), yoff);
                        Data[SectionName.Party].ITEM[pos, 4] = new IGMDataItem_Int(num, r, 2, 0, 1);

                        r = dims.Item3;
                        r.Offset(437, yoff);
                        Data[SectionName.Party].ITEM[pos, 5] = new IGMDataItem_Icon(Icons.ID.Slash_Forward, r, 13);

                        r = dims.Item3;

                        num = Memory.State.Party[0] == character ||
                            Memory.State.Party[1] == character && Memory.State.Party[0] == Saves.Characters.Blank ||
                            Memory.State.Party[2] == character && Memory.State.Party[0] == Saves.Characters.Blank && Memory.State.Party[1] == Saves.Characters.Blank
                            ? Memory.State.firstcharactersmaxHP : 0;
                        spaces = 4 - num.ToString().Length;
                        r.Offset((459 + spaces * 20), yoff);
                        Data[SectionName.Party].ITEM[pos, 6] = new IGMDataItem_Int(num, r, 2, 0, 1);

                        Data[SectionName.Non_Party].BLANKS[pos] = false;
                    }
                    else
                    {
                        Data[SectionName.Party].ITEM[pos, 0] = new IGMDataItem_Box(pos: Data[SectionName.Party].SIZE[pos]);
                        Data[SectionName.Non_Party].BLANKS[pos] = true;
                        for (int i = 1; i < Data[SectionName.Party].Depth; i++)
                        {
                            Data[SectionName.Party].ITEM[pos, i] = null;
                        }
                    }
                }
            }

            private void Update_PartyStatus_Boxes()
            {
                for (sbyte i = 0; Memory.State.Party != null && i < Data[SectionName.Party].SIZE.Length; i++)
                    Update_PartyStatus_Box(i, Memory.State.Party[i]);
            }

            public override bool Update()
            {
                Vector2 Zoom = Memory.Scale(Size.X, Size.Y, Memory.ScaleMode.FitBoth);

                focus = Matrix.CreateTranslation((Size.X / -2), (Size.Y / -2), 0) *
                    Matrix.CreateScale(new Vector3(Zoom.X, Zoom.Y, 1)) *
                    Matrix.CreateTranslation(vp.X / 2, vp.Y / 2, 0);
                //todo detect when there is no saves detected.
                //check for null
                foreach(var i in Data)
                {
                    i.Value.Update();
                }
                ((IGMData_Header)Data[SectionName.Header]).Update(choSideBar);
                Update_PartyStatus_Boxes();

                return Inputs();
            }

            protected override bool Inputs()
            {
                bool ret = false;
                ml = Input.MouseLocation.Transform(focus);

                if (mode == Mode.ChooseItem)
                {
                    if (Data[SectionName.SideMenu] != null && Data[SectionName.SideMenu].Count > 0)
                    {
                        for (int pos = 0; pos < Data[SectionName.SideMenu].Count; pos++)
                        {
                            Rectangle r = Data[SectionName.SideMenu].ITEM[pos, 0];
                            //r.Offset(focus.Translation.X, focus.Translation.Y);
                            if (r.Contains(ml))
                            {
                                choSideBar = (Items)pos;
                                ret = true;

                                if (Input.Button(Buttons.MouseWheelup) || Input.Button(Buttons.MouseWheeldown))
                                {
                                    return ret;
                                }
                                break;
                            }
                        }

                        if (Input.Button(Buttons.Down))
                        {
                            Input.ResetInputLimit();
                            init_debugger_Audio.PlaySound(0);
                            if (++choSideBar > Enum.GetValues(typeof(Items)).Cast<Items>().Max())
                                choSideBar = Enum.GetValues(typeof(Items)).Cast<Items>().Min();
                            ret = true;
                        }
                        else if (Input.Button(Buttons.Up))
                        {
                            Input.ResetInputLimit();
                            init_debugger_Audio.PlaySound(0);
                            if (--choSideBar < 0)
                                choSideBar = Enum.GetValues(typeof(Items)).Cast<Items>().Max();
                            ret = true;
                        }
                        else if (Input.Button(Buttons.Cancel))
                        {
                            Input.ResetInputLimit();
                            init_debugger_Audio.PlaySound(8);
                            Fade = 0.0f;
                            State = MainMenuStates.LoadGameChooseGame;
                            ret = true;
                        }
                        else if (Input.Button(Buttons.Okay))
                        {
                            Input.ResetInputLimit();
                            init_debugger_Audio.PlaySound(0);
                            ret = true;
                            switch (choSideBar)
                            {
                                //Select Char Mode
                                case Items.Junction:
                                case Items.Magic:
                                case Items.Status:
                                    mode = Mode.ChooseChar;
                                    break;
                            }
                        }
                    }
                }
                else if (mode == Mode.ChooseChar)
                {
                    for (int i = 0; i < Data[SectionName.Party].Count; i++)
                    {
                        if (Data[SectionName.Party].BLANKS[i]) continue;
                        Rectangle r = Data[SectionName.Party].SIZE[i];
                        //r.Offset(focus.Translation.X, focus.Translation.Y);
                        if (r.Contains(ml))
                        {
                            choChar = i;
                            ret = true;

                            if (Input.Button(Buttons.MouseWheelup) || Input.Button(Buttons.MouseWheeldown))
                            {
                                return ret;
                            }
                            break;
                        }
                    }
                    for (int i = Data[SectionName.Party].Count; i < Data[SectionName.Non_Party].Count + Data[SectionName.Party].Count; i++)
                    {
                        if (Data[SectionName.Non_Party].BLANKS[i - Data[SectionName.Party].Count]) continue;
                        Rectangle r = Data[SectionName.Non_Party].SIZE[i - Data[SectionName.Party].Count];
                        //r.Offset(focus.Translation.X, focus.Translation.Y);
                        if (r.Contains(ml))
                        {
                            choChar = i;
                            ret = true;

                            if (Input.Button(Buttons.MouseWheelup) || Input.Button(Buttons.MouseWheeldown))
                            {
                                return ret;
                            }
                            break;
                        }
                    }
                    if (Input.Button(Buttons.Down))
                    {
                        Input.ResetInputLimit();
                        init_debugger_Audio.PlaySound(0);
                        choChar++;
                        ret = true;
                    }
                    else if (Input.Button(Buttons.Up))
                    {
                        Input.ResetInputLimit();
                        init_debugger_Audio.PlaySound(0);
                        choChar--;
                        ret = true;
                    }
                    else if (Input.Button(Buttons.Cancel))
                    {
                        Input.ResetInputLimit();
                        ret = true;
                        init_debugger_Audio.PlaySound(8);
                        mode = Mode.ChooseItem;
                    }
                }
                return ret;
            }

            protected class IGMData_Header : IGMData
            {
                private Dictionary<Enum, Item> strHeaderText;

                public IGMData_Header(int count, int depth, IGMDataItem container = null) : base(count, depth, container)
                {
                }

                //public override void Draw() => base.Draw();

                public override void Init()
                {
                    base.Init();
                    strHeaderText = new Dictionary<Enum, Item>()
                    {
                    { Items.Junction, new Item{Text=Memory.Strings.Read(Strings.FileID.MNGRP, 0 ,1) } },
                    { Items.Item, new Item{Text=Memory.Strings.Read(Strings.FileID.MNGRP, 0 ,3) } },
                    { Items.Magic, new Item{Text=Memory.Strings.Read(Strings.FileID.MNGRP, 0 ,5) } },
                    { Items.Status, new Item{Text=Memory.Strings.Read(Strings.FileID.MNGRP, 0 ,9) } },
                    { Items.GF, new Item{Text=Memory.Strings.Read(Strings.FileID.MNGRP, 0 ,7) } },
                    { Items.Ability, new Item{Text=Memory.Strings.Read(Strings.FileID.MNGRP, 0 ,63) } },
                    { Items.Switch, new Item{Text=Memory.Strings.Read(Strings.FileID.MNGRP, 0 ,65) } },
                    { Items.Card, new Item{Text=Memory.Strings.Read(Strings.FileID.MNGRP, 0 ,11) } },
                    { Items.Config, new Item{Text=Memory.Strings.Read(Strings.FileID.MNGRP, 0 ,17) } },
                    { Items.Tutorial, new Item{Text=Memory.Strings.Read(Strings.FileID.MNGRP, 0 ,68) } },
                    { Items.Save, new Item{Text=Memory.Strings.Read(Strings.FileID.MNGRP, 0 ,15) } },
                    };
                }
                
                public bool Update(IGM.Items selection)
                {
                    ((IGMDataItem_Box)CONTAINER).Data = strHeaderText[selection];
                    return true;
                }

                private new bool Update() => base.Update();
            }

            private class IGMData_Footer : IGMData
            {
                public IGMData_Footer(int count, int depth, IGMDataItem container = null) : base(count, depth, container)
                {
                }

                public override bool Update()
                {
                    base.Update();
                    ((IGMDataItem_Box)CONTAINER).Data = Memory.Strings.Read(Strings.FileID.AREAMES, 0, Memory.State.LocationID).ReplaceRegion();
                    return true;
                }
            }

            private class IGMData_Clock : IGMData
            {
                public IGMData_Clock(int count, int depth, IGMDataItem container = null) : base(count, depth, container)
                {
                }

                public override void Init()
                {
                    base.Init();
                    Rectangle r;
                    r = CONTAINER;
                    r.Offset(25, 14);
                    ITEM[0, 0] = new IGMDataItem_Icon(Icons.ID.PLAY, r, 13);

                    r = CONTAINER;
                    r.Offset(145, 14);
                    ITEM[0, 2] = new IGMDataItem_Icon(Icons.ID.Colon, r, 13, 2, .5f);

                    r = CONTAINER;
                    r.Offset(25, 48);
                    ITEM[0, 4] = new IGMDataItem_Icon(Icons.ID.SeeD, r, 13);

                    r = CONTAINER;
                    r.Offset(185, 81);
                    ITEM[0, 7] = new IGMDataItem_Icon(Icons.ID.G, r, 2);
                }

                public override bool Update()
                {
                    bool ret = base.Update();
                    int num;
                    int spaces;
                    Rectangle r;

                    r = CONTAINER;
                    num = Memory.State.timeplayed.TotalHours < 99 ? (int)(Memory.State.timeplayed.TotalHours) : 99;
                    spaces = 2 - (num).ToString().Length;
                    r.Offset(105 + spaces * 20, 14);
                    ITEM[0, 1] = new IGMDataItem_Int(num, r, 2, 0, 1);

                    r = CONTAINER;
                    num = num >= 99 ? 99 : Memory.State.timeplayed.Minutes;
                    spaces = 0;
                    r.Offset(165 + spaces * 20, 14);
                    ITEM[0, 3] = new IGMDataItem_Int(num, r, 2, 0, 2);

                    r = CONTAINER;
                    num = Memory.State.Fieldvars.SeedRankPts / 100;
                    num = num < 99999 ? num : 99999;
                    spaces = 5 - (num).ToString().Length;
                    r.Offset(105 + spaces * 20, 48);
                    ITEM[0, 5] = new IGMDataItem_Int(num, r, 2, 0, 1);

                    r = CONTAINER;
                    num = Memory.State.AmountofGil < 99999999 ? (int)(Memory.State.AmountofGil) : 99999999;
                    spaces = 8 - (num).ToString().Length;
                    r.Offset(25 + spaces * 20, 81);
                    ITEM[0, 6] = new IGMDataItem_Int(num, r, 2, 0, 1);
                    return true;
                }
            }

            private class IGMData_NonParty : IGMData
            {
                private Texture2D _red_pixel;

                public IGMData_NonParty(int count, int depth, IGMDataItem container = null) : base(count, depth, container)
                {
                }

                public override void Init()
                {
                    base.Init();
                    _red_pixel = new Texture2D(Memory.graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                    Color[] color = new Color[] { new Color(74.5f / 100, 12.5f / 100, 11.8f / 100, 100) };
                    _red_pixel.SetData<Color>(color, 0, _red_pixel.Width * _red_pixel.Height);
                    int row = 0, col = 0;
                    for (int i = 0; i < SIZE.Length; i++)
                    {
                        int width = Width / 2;
                        int height = Height / 3;
                        row = i / 2;
                        col = i % 2;
                        SIZE[i] = new Rectangle
                        {
                            Width = width,
                            Height = height,
                            X = X + col * width,
                            Y = Y + row * height
                        };
                        SIZE[i].Inflate(-26, -8);
                        if (i > 1) SIZE[i].Y -= 8;
                    }
                }

                public override bool Update()
                {
                    sbyte pos = 0;
                    bool ret = base.Update();
                    for (byte i = 0; Memory.State.Party != null && i <= (byte)Faces.ID.Edea_Kramer && SIZE != null && pos < SIZE.Length; i++)
                    {
                        if (!Memory.State.Party.Contains((Saves.Characters)i) && Memory.State.Characters[i].Exists != 0 && Memory.State.Characters[i].Exists != 6)//15,9,7,4 shows on menu, 0 locked, 6 hidden
                        {
                            BLANKS[pos] = false;
                            Update(pos++, (Saves.Characters)i);
                        }
                    }
                    for (; pos < Count; pos++)
                    {
                        for (int i = 0; i < Depth; i++)
                        {
                            BLANKS[pos] = true;
                            ITEM[pos, i] = null;
                        }
                    }
                    return true;
                }
                private void Update(sbyte pos, Saves.Characters character)
                {
                    float yoff = 39;
                    int num = 0;
                    int spaces = 0;
                    Rectangle rbak = SIZE[pos];
                    Rectangle r = rbak;
                    Color color = new Color(74.5f / 100, 12.5f / 100, 11.8f / 100, .9f);
                    ITEM[pos, 0] = new IGMDataItem_String(Memory.Strings.GetName((Faces.ID)character), rbak);
                    CURSOR[pos] = new Point(rbak.X, (int)(rbak.Y + (6 * TextScale.Y)));

                    r.Offset(7, yoff);
                    ITEM[pos, 1] = new IGMDataItem_Icon(Icons.ID.Lv, r, 13);

                    r = rbak;
                    num = Memory.State.Characters[(int)character].Level;
                    spaces = 3 - num.ToString().Length;
                    r.Offset((49 + spaces * 20), yoff);
                    ITEM[pos, 2] = new IGMDataItem_Int(num, r, 2, 0, 1);

                    r = rbak;
                    r.Offset(126, yoff);
                    ITEM[pos, 3] = new IGMDataItem_Icon(Icons.ID.HP2, r, 13);

                    r.Offset(0, 28);
                    r.Width = 118;
                    r.Height = 1;
                    ITEM[pos, 4] = new IGMDataItem_Texture(_red_pixel, r) { Color = color };

                    r.Offset(0, 2);
                    ITEM[pos, 5] = new IGMDataItem_Texture(_red_pixel, r) { Color = color };

                    r = rbak;
                    num = Memory.State.Characters[(int)character].CurrentHP;
                    spaces = 4 - num.ToString().Length;
                    r.Offset((166 + spaces * 20), yoff);
                    ITEM[pos, 6] = new IGMDataItem_Int(num, r, 2, 0, 1);
                }
            }

            private class IGMData_Party : IGMData
            {
                public IGMData_Party(int count, int depth, IGMDataItem container = null) : base(count, depth, container)
                {
                }

                public override void Init()
                {

                    base.Init();
                    for (int i = 0; i < 3; i++)
                        SIZE[i] = new Rectangle { Width = 580, Height = 78, X = 20, Y = 84 + 78 * i };
                }

                public override bool Update()
                {
                    return base.Update();
                }
            }

            private class IGMDataItem_Empty : IGMDataItem
            {
                public IGMDataItem_Empty(Rectangle? pos = null) : base(pos)
                {
                }

                public override void Draw() { }
            }

            private class IGMData_SideMenu : IGMData
            {
                public IGMData_SideMenu(int count, int depth, IGMDataItem container = null) : base(count, depth, container)
                {
                }

                public override void Init()
                {
                    base.Init();
                    ITEM[0, 0] = new IGMDataItem_String(Memory.Strings.Read(Strings.FileID.MNGRP, 0, 0));
                    ITEM[1, 0] = new IGMDataItem_String(Memory.Strings.Read(Strings.FileID.MNGRP, 0, 2));
                    ITEM[2, 0] = new IGMDataItem_String(Memory.Strings.Read(Strings.FileID.MNGRP, 0, 4));
                    ITEM[3, 0] = new IGMDataItem_String(Memory.Strings.Read(Strings.FileID.MNGRP, 0, 8));
                    ITEM[4, 0] = new IGMDataItem_String(Memory.Strings.Read(Strings.FileID.MNGRP, 0, 6));
                    ITEM[5, 0] = new IGMDataItem_String(Memory.Strings.Read(Strings.FileID.MNGRP, 0, 62));
                    ITEM[6, 0] = new IGMDataItem_String(Memory.Strings.Read(Strings.FileID.MNGRP, 0, 64));
                    ITEM[7, 0] = new IGMDataItem_String(Memory.Strings.Read(Strings.FileID.MNGRP, 0, 10));
                    ITEM[8, 0] = new IGMDataItem_String(Memory.Strings.Read(Strings.FileID.MNGRP, 0, 16));
                    ITEM[9, 0] = new IGMDataItem_String(Memory.Strings.Read(Strings.FileID.MNGRP, 0, 67));
                    ITEM[10, 0] = new IGMDataItem_String(Memory.Strings.Read(Strings.FileID.MNGRP, 0, 14));
                    for (int i = 0; i < Count; i++)
                    {
                        Rectangle r = new Rectangle
                        {
                            Width = Width,
                            Height = Height / Count,
                            X = X,
                            Y = Y + (Height / Count) * i,
                        };
                        r.Inflate(-26, -12);
                        ITEM[i, 0].Pos = r;
                        CURSOR[i] = new Point(r.X, (int)(r.Y + 6 * TextScale.Y));
                    }
                }

                public override bool Inputs()
                {
                    return base.Inputs();
                }

                public override bool Update()
                {
                    return base.Update();
                }
            }

            #endregion Methods
        }

        #region Classes

        public class IGMData
        {
            #region Fields

            /// <summary>
            /// location of where pointer finger will point.
            /// </summary>
            public Point[] CURSOR;

            public IGMDataItem[,] ITEM;

            /// <summary>
            /// Size of the entire area
            /// </summary>
            public Rectangle[] SIZE;

            public bool[] BLANKS;
            public IGMDataItem CONTAINER;

            #endregion Fields

            #region Constructors

            public IGMData(int count, int depth, IGMDataItem container = null)
            {
                SIZE = new Rectangle[count];
                ITEM = new IGMDataItem[count, depth];
                CURSOR = new Point[count];

                Count = (byte)count;
                Depth = (byte)depth;
                BLANKS = new bool[count];
                CONTAINER = container;
                Init();
            }

            public IGMDataItem this[int pos, int i] { get => ITEM[pos, i]; set => ITEM[pos, i] = value; }

            public virtual void Draw()
            {
                if (CONTAINER != null)
                    CONTAINER.Draw();
                foreach (IGMDataItem i in ITEM)
                {
                    if (i != null)
                        i.Draw();
                }
            }

            #endregion Constructors

            #region Properties

            public byte Count { get; private set; }
            public byte Depth { get; private set; }
            public int Width => CONTAINER != null ? CONTAINER.Pos.Width : 0;
            public int Height => CONTAINER != null ? CONTAINER.Pos.Height : 0;
            public int X => CONTAINER != null ? CONTAINER.Pos.X : 0;
            public int Y => CONTAINER != null ? CONTAINER.Pos.Y : 0;

            public static implicit operator Rectangle(IGMData v) => v.CONTAINER ?? Rectangle.Empty;

            public virtual bool Update() { return false; }
            public virtual bool Inputs()
            {
                return false;
            }
            public virtual void Init()
            { }

            #endregion Properties
        }

        public abstract class IGMDataItem//<T>
        {
            //protected T _data;
            protected Rectangle _pos;

            public IGMDataItem(Rectangle? pos = null) =>
                //_data = data;
                _pos = pos ?? Rectangle.Empty;

            /// <summary>
            /// Dynamic data that is passed from update to draw.
            /// </summary>
            //public virtual T Data { get => _data; set => _data = value; }
            /// <summary>
            /// Where to draw this item.
            /// </summary>
            public virtual Rectangle Pos
            {
                get => _pos; set => _pos = value;

                //public implicit operator IGMDataItem<T>(IGMDataItem_Icon v) => throw new NotImplementedException();
            }

            public Color Color { get; internal set; } = Color.White;

            //public virtual object Data { get; internal set; }
            //public virtual FF8String Data { get; internal set; }
            public abstract void Draw();

            public static implicit operator Rectangle(IGMDataItem v) => v.Pos;

            public static implicit operator Color(IGMDataItem v) => v.Color;
        }

        public class IGMDataItem_Icon : IGMDataItem//<Icons.ID>
        {
            private byte _pallet;
            private byte _faded_pallet;

            public Icons.ID Data { get; set; }

            public byte Pallet
            {
                get => _pallet; set
                {
                    if (value >= 16) value = 2;
                    _pallet = value;
                }
            }

            public byte Faded_Pallet
            {
                get => _faded_pallet; set
                {
                    if (value >= 16) value = 2;
                    _faded_pallet = value;
                }
            }

            public bool Blink => Faded_Pallet != Pallet;
            public float Blink_Adjustment { get; set; }

            public IGMDataItem_Icon(Icons.ID data, Rectangle? pos = null, byte? pallet = null, byte? faded_pallet = null, float blink_adjustment = 1f) : base(pos)
            {
                Data = data;
                Pallet = pallet ?? 2;
                Faded_Pallet = faded_pallet ?? Pallet;
                Blink_Adjustment = blink_adjustment;
            }

            public override void Draw()
            {
                Memory.Icons.Draw(Data, Pallet, Pos, TextScale, fade);
                if (Blink)
                    Memory.Icons.Draw(Data, Faded_Pallet, Pos, TextScale, fade * blink_Amount * Blink_Adjustment);
            }
        }

        public class IGMDataItem_Int : IGMDataItem//<Int>
        {
            private byte _pallet;

            public int Data { get; set; }
            public byte Padding { get; set; }

            public byte Pallet
            {
                get => _pallet; set
                {
                    if (value >= 16) value = 2;
                    _pallet = value;
                }
            }

            public Icons.NumType NumType { get; set; }

            public IGMDataItem_Int(int data, Rectangle? pos = null, byte? pallet = null, Icons.NumType? numtype = null, byte? padding = null) : base(pos)
            {
                Data = data;
                Padding = padding ?? 1;
                Pallet = pallet ?? 2;
                NumType = numtype ?? 0;
            }

            public override void Draw() => Memory.Icons.Draw(Data, NumType, Pallet, $"D{Padding}", Pos.Location.ToVector2(), TextScale, fade);
        }

        private class IGMDataItem_String : IGMDataItem
        {
            public FF8String Data { get; set; }

            public IGMDataItem_String(FF8String data, Rectangle? pos = null) : base(pos) => this.Data = data;

            public override void Draw() => Memory.font.RenderBasicText(Data, Pos.Location, TextScale, Fade: fade);
        }

        private class IGMDataItem_Box : IGMDataItem
        {
            public FF8String Data { get; set; }
            public Icons.ID? Title { get; set; }
            public bool Indent { get; set; }

            public IGMDataItem_Box(FF8String data = null, Rectangle? pos = null, Icons.ID? title = null, bool indent = false) : base(pos)
            {
                Data = data;
                Title = title;
                Indent = indent;
            }

            public override void Draw() =>
                    DrawBox(Pos, Data, Title, indent: Indent);
        }

        private class IGMDataItem_Texture : IGMDataItem
        {
            public Texture2D Data { get; set; }

            public IGMDataItem_Texture(Texture2D data, Rectangle? pos = null) : base(pos) => this.Data = data;

            public override void Draw() => Memory.spriteBatch.Draw(Data, Pos, null, base.Color * fade);//4
        }

        #endregion Classes

        private abstract class Menu
        {
            protected Dictionary<Enum, IGMData> Data;

            public Menu()
            {
                Data = new Dictionary<Enum, IGMData>();
                Init();
            }

            protected abstract void Init();

            public abstract void Draw();

            public abstract bool Update();

            protected abstract bool Inputs();
        }
    }
}