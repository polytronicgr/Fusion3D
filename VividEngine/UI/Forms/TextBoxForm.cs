using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid3D.Texture;
using Vivid3D.Draw;
using OpenTK;

namespace Vivid3D.Resonance.Forms
{
    public class TextBoxForm : UIForm
    {
        public int ClaretI = 0;
        public bool Active = false;
        public int StartI = 0;
        public bool ShowClaret = false;
        public int NextClaret;
        public int ClaretE = 0;
       
        public TextBoxForm()
        {

            void KeyPressFunc(OpenTK.Input.Key key,bool shift)
            {
                string k = "";
                switch (key)
                {
                    case OpenTK.Input.Key.Right:
                        if (Text.Length == 1)
                        {
                            if (ClaretI == 0)
                            {
                                ClaretI = 1;
                                return;
                            }
                        }
                        if (Text.Length == 0)
                        {
                            return;
                        }
                        if (Text.Length > 1)
                        {
                            if (ClaretI < Text.Length)
                            {
                                ClaretI++;
                            }
                        }
                        if (ClaretI > ClaretE)
                        {
                            if (StartI < Text.Length)
                            {
                                StartI++;
                            }
                        }
                        return;
                        break;
                    case OpenTK.Input.Key.Left:
                    
                        if (ClaretI == StartI)
                        {
                            if (StartI > 0)
                            {
                                StartI--;
                            }
                        }
                        if (Text.Length == 1)
                        {
                            ClaretI = 0;
                            return;
                        }
                        if (Text.Length == 0) return;
                        if (Text.Length > 1)
                        {
                            if (ClaretI > 0)
                            {
                                ClaretI--;
                                return;
                            }
                        }
                        return;
                        break;
                    case OpenTK.Input.Key.BackSpace:

                        if(Text.Length == 1)
                        {
                            ClaretI = 0;
                            Text = "";
                            return;
                        }
                        if (Text.Length > 1)
                        {

                            if(ClaretI == Text.Length)
                            {
                                Text = Text.Substring(0, Text.Length - 1);
                                ClaretI--;
                                return;
                            }
                            var os = Text.Substring(0, ClaretI - 1) + Text.Substring(ClaretI);
                            Text = os;
                            ClaretI--;
                            return;


                        }

                        break;
                    case OpenTK.Input.Key.Delete:
                        if(Text.Length==1 && ClaretI == 0)
                        {
                            Text = "";
                            ClaretI = 0;
                            return;
                        }
                        if (Text.Length > 1 && ClaretI < Text.Length)
                        {
                            if ((Text.Length - ClaretI) > 1)
                            {
                                Text = Text.Substring(0, ClaretI)+Text.Substring(ClaretI+1);
                            }
                            else
                            {
                                Text = Text.Substring(0, Text.Length - 1);
                            }
                            return;
                        }
                        return;
                        break;
                    case OpenTK.Input.Key.Space:

                        k = " ";
                        break;
                    default:
                        k = shift ? key.ToString().ToUpper() : key.ToString().ToLower();
                        break;
                }
                if (Text.Length == 0)
                {
                    Text = k;
                    ClaretI = 1;
                }
                else
                {
                    if(ClaretI == Text.Length)
                    {
                        Text = Text + k;
                        ClaretI++;

                        int iv = StartI;
                        var sw = Text.Substring(iv);
                        if (UI.Font.Width(sw) > W)
                        {
                            StartI++;
                        }
                        return;
                    }
                    var os = Text.Substring(0, ClaretI) + k;
                    if (Text.Length > ClaretI)
                    {
                        os = os + Text.Substring(ClaretI);
                    }
                    Text = os;
                    ClaretI++;
                }
            }
            KeyPress = KeyPressFunc;

            void UpdateFunc()
            {
                if (Active)
                {
                    if (Environment.TickCount > NextClaret)
                    {
                        ShowClaret = ShowClaret ? false : true;
                        NextClaret =Environment.TickCount + 450;
                        Console.WriteLine("Claret:" + ShowClaret.ToString());
                    }
                }
               
            }

            Update = UpdateFunc;

            void ActiveFunc()
            {
                Active = true;
            }

            void DeactiveFunc()
            {
                Active = false;
            }

            Deactivate = DeactiveFunc;
            Activate = ActiveFunc;

            void DrawFunc()
            {

                DrawFormSolid(Col);
                int tw = 0;
                int ii = 0;
                int vc = 0;

                /*
                for (int i = StartI; i < Text.Substring(StartI).Length; i++)
                {
                    tw += UI.Font.Width(Text.Substring(i, 1));
                    if (tw > W - 10) break;
                    ii++;
                }
                */
                //vc = ClaretI - StartI;

                string dis=Text.Substring(StartI);

                int cc = 0;
                int t2 = 0;
                string rtxt = "";
                for(int i = 0; i < dis.Length; i++)
                {
                    rtxt = rtxt + dis.Substring(i, 1);
                    var cr = dis.Substring(i, 1);
                    t2 += UI.Font.Width(cr);
                    if (t2 > W-30)
                    {
                        break;
                    }
                    cc++;
                }
                ClaretE = cc;
                
                DrawText(rtxt, 5, 0, new Vector4(0.2f, 0.2f, 0.2f, 0.9f));

                if (Text.Length == 0) ClaretI = 0;

                
                if(ShowClaret)
                {
                    // Console.WriteLine("Claret!");
                    int cx = 0;
                    if (Text.Length > 0)
                    {
                        int cv = 0;
                        for (int i = StartI; i < ClaretI; i++)
                        {

                            int cw = UI.Font.Width(Text.Substring(i, 1));
                            cx = cx + (cw);
                            cv++;
                            if (cv > cc) break;
                        }
                    }
                    DrawFormSolid(new Vector4(0.2f, 0.2f, 0.2f, 0.8f),cx+(Text.Length>0 ? 5 : 0),0,5, 20);
                }

            }

            Draw = DrawFunc;

        }

    }
}
