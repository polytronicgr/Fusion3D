using System;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace VividEdit.Forms
{
    public partial class ClassInspector : DockContent
    {
        public Inspectors.InspectorBase IC = null;
        public Inspectors.InspectorBase Inspecting;

        public ClassInspector ( )
        {
            InitializeComponent ( );
        }

        public void BeginInspect ( )
        {
            if ( Inspecting != null )
            {
                Inspecting.Inspecting = true;
                Inspecting.AlignV ( );
            }
        }

        public void EndInspect ( )
        {
            if ( Inspecting != null )
            {
                Inspecting.Inspecting = false;
            }
        }

        public void Inspect ( object cls )
        {
            Controls.Clear ( );
            Label header = new Label
            {
                Text = "Inspecting:" + cls.GetType ( ).Name ,
                Location = new Point ( 5 , 5 ) ,
                Size = new Size ( 120 , 25 )
            };
            Controls.Add ( header );

            Type type = cls.GetType();

            int propy = 30;

            foreach ( System.Reflection.PropertyInfo prop in type.GetProperties ( ) )
            {
                Label prop_header = new Label
                {
                    Text = prop.Name ,
                    Location = new Point ( 5 , propy ) ,
                    Size = new Size ( 250 , 25 )
                };

                if ( prop.PropertyType == typeof ( System.Collections.Generic.List<Vivid3D.Script.ScriptLink> ) )
                {
                    Label val = new Label
                    {
                        Text = prop.Name,
                        Location = new Point(5,propy+25),
                        Size = new Size(50,25)
                    };

                    Button news = new Button
                    {
                        Text = "New" ,
                        Location = new Point ( 60 , propy + 25 ) ,
                        Size = new Size ( 60 , 25 )
                    };
                    System.Collections.Generic.List<Vivid3D.Script.ScriptLink> sv = (System.Collections.Generic.List<Vivid3D.Script.ScriptLink>)prop.GetAccessors()[0].Invoke(cls,null);
                    System.Reflection.MethodInfo sm = prop.GetSetMethod ( );
                    object[] pp = new object[1];

                    void on_new ( object o, EventArgs e )
                    {
                        Vivid3D.Script.ScriptLink sb = new Vivid3D.Script.ScriptLink ( );
                        sv.Add ( sb );

                        sb.Name = "NewScript.cs";
                        sb.FilePath = VividEdit.VividED.CurProject.ContentPath + "\\Script\\NewScript.cs";

                        System.IO.File.Create ( VividEdit.VividED.CurProject.ContentPath + "\\Script\\NewScript.cs" ).Close ( );

                        pp [ 0 ] = sv;

                        sm?.Invoke ( cls, pp );

                        Inspect ( cls );
                    }

                    propy += 25;

                    news.Click += on_new;

                    propy += 10;

                    foreach ( Vivid3D.Script.ScriptLink s in sv )
                    {
                        TextBox namebox = new TextBox
                        {
                            Location = new Point ( 5 , propy + 25 ) ,
                            Size = new Size ( 90 , 25 ) ,
                            Text = s.Name
                        };

                        void on_tc ( object o, EventArgs e )
                        {
                            if ( namebox.Text == "" || namebox.Text == " " || namebox.Text == "    " )
                            {
                                return;
                            }
                            s.Name = namebox.Text;

                            if ( System.IO.File.Exists ( VividEdit.VividED.CurProject.ContentPath + "\\Script\\" + namebox.Text ) )
                            {
                                // System.IO.File.Delete ( VividEdit.VividED.CurProject.ContentPath +
                                // "\\Script\\" + namebox.Text );
                            }

                            System.IO.File.Move ( s.FilePath, VividEdit.VividED.CurProject.ContentPath + "\\Script\\" + namebox.Text );
                            s.FilePath = VividEdit.VividED.CurProject.ContentPath + "\\Script\\" + s.Name;
                        }
                        namebox.TextChanged += on_tc;

                        Controls.Add ( namebox );
                        Button del = new Button
                        {
                            Location = new Point ( 105 , propy + 25 ) ,
                            Size = new Size ( 60 , 25 ),
                            Text = "Delete"
                        };
                        Controls.Add ( del );

                        void del_s ( object o, EventArgs e )
                        {
                            System.IO.File.Delete ( s.FilePath );
                            sv.Remove ( s );
                            Inspect ( cls );
                        }

                        del.Click += del_s;

                        propy += 25;
                    }

                    Controls.Add ( val );
                    Controls.Add ( news );
                }
                else if ( prop.PropertyType == typeof ( string ) )
                {
                    Label val = new Label
                    {
                        Text = prop.Name,
                        Location = new Point(5,propy+25),
                        Size = new Size(50,25)
                    };

                    TextBox boxv = new TextBox
                    {
                        Location = new Point( 60,propy+22),
                        Size = new Size( 120,25 )
                    };

                    string sv = (string)prop.GetAccessors()[0].Invoke(cls,null);
                    System.Reflection.MethodInfo sm = prop.GetSetMethod ( );
                    object[] pp = new object[1];

                    boxv.Text = sv;

                    void c_val ( object o, EventArgs e )
                    {
                        try
                        {
                            sv = boxv.Text;
                        }
                        catch
                        {
                            sv = "";
                        }

                        if ( sm != null )
                        {
                            pp [ 0 ] = sv;
                            sm.Invoke ( cls, pp );
                        }
                    }
                    boxv.TextChanged += c_val;
                    Controls.Add ( val );
                    Controls.Add ( boxv );
                }
                else if ( prop.PropertyType == typeof ( float ) )
                {
                    Label val = new Label
                    {
                        Text = prop.Name ,
                        Location = new Point ( 5 , propy + 25 ) ,
                        Size = new Size ( 40 , 25 )
                    };

                    TextBox boxv = new TextBox
                    {
                        Location = new Point ( 60 , propy + 22 ) ,
                        Size = new Size ( 120 , 25 )
                    };

                    float v3 = (float)prop.GetAccessors()[0].Invoke(cls, null);
                    System.Reflection.MethodInfo sm = prop.GetSetMethod ( );

                    object[] pp = new object[1];

                    boxv.Text = v3.ToString ( );

                    void c_val ( object o, EventArgs e )
                    {
                        try
                        {
                            v3 = float.Parse ( boxv.Text );
                        }
                        catch
                        {
                            v3 = 0;
                        }
                        if ( sm != null )
                        {
                            pp [ 0 ] = v3;
                            sm.Invoke ( cls, pp );
                        }
                    }

                    boxv.TextChanged += c_val;

                    Controls.Add ( val );
                    Controls.Add ( boxv );
                }
                else if ( prop.PropertyType == typeof ( OpenTK.Vector3 ) )
                {
                    Label labx, laby, labz;
                    labx = new Label ( );
                    laby = new Label ( );
                    labz = new Label ( );

                    TextBox boxx, boxy, boxz;

                    boxx = new TextBox ( );
                    boxy = new TextBox ( );
                    boxz = new TextBox ( );

                    labx.Location = new Point ( 5, propy + 25 );
                    laby.Location = new Point ( 120, propy + 25 );
                    labz.Location = new Point ( 235, propy + 25 );

                    labx.Size = new Size ( 15, 25 );
                    laby.Size = new Size ( 15, 25 );
                    labz.Size = new Size ( 15, 25 );
                    labx.Text = "X";
                    laby.Text = "Y";
                    labz.Text = "Z";

                    OpenTK.Vector3 v3 = ( OpenTK.Vector3 ) prop.GetAccessors ( ) [ 0 ].Invoke ( cls , null );
                    System.Reflection.MethodInfo sm = prop.GetSetMethod ( );

                    object[] pp = new object[1];

                    Controls.Add ( labx ); Controls.Add ( laby ); Controls.Add ( labz );

                    void v3_cx ( object s, EventArgs a )
                    {
                        try
                        {
                            v3.X = float.Parse ( boxx.Text );
                        }
                        catch
                        {
                            v3.X = 0;
                        }
                        if ( sm != null )
                        {
                            pp [ 0 ] = v3;
                            sm.Invoke ( cls, pp );
                        }
                        //prop.GetAccessors()[1].Invoke(cls, pp);
                    }

                    void v3_cy ( object s, EventArgs e )
                    {
                        try
                        {
                            v3.Y = float.Parse ( boxy.Text );
                        }
                        catch
                        {
                            v3.Y = 0;
                        }
                        if ( sm != null )
                        {
                            pp [ 0 ] = v3;
                            sm.Invoke ( cls, pp );
                        }
                    }

                    void v3_cz ( object s, EventArgs e )
                    {
                        try
                        {
                            v3.Z = float.Parse ( boxz.Text );
                        }
                        catch
                        {
                            v3.Z = 0;
                        }
                        if ( sm != null )
                        {
                            pp [ 0 ] = v3;
                            sm.Invoke ( cls, pp );
                        }
                    }

                    boxx.TextChanged += v3_cx;
                    boxy.TextChanged += v3_cy;
                    boxz.TextChanged += v3_cz;

                    boxx.Location = new Point ( 20, propy + 22 );
                    boxy.Location = new Point ( 135, propy + 22 );
                    boxz.Location = new Point ( 250, propy + 22 );

                    boxx.Size = new Size ( 80, 25 );
                    boxy.Size = new Size ( 80, 25 );
                    boxz.Size = new Size ( 80, 25 );

                    Controls.Add ( boxx ); Controls.Add ( boxy ); Controls.Add ( boxz );

                    boxx.Text = v3.X.ToString ( ); boxy.Text = v3.Y.ToString ( ); boxz.Text = v3.Z.ToString ( );

                    Controls.Add ( prop_header );
                }
                else
                {
                    continue;
                }

                propy += 50;
            }

            return;
            if ( cls is Vivid3D.Material.Material3D )
            {
                Vivid3D.Material.Material3D cls3 = cls as Vivid3D.Material.Material3D;
                Text = "Inspecting Material";
                if ( IC != null )
                {
                    Controls.Remove ( IC );
                }
                Inspectors.InspectorMaterial mi = new Inspectors.InspectorMaterial ( );
                Inspecting = mi;
                mi.Mat = cls3;
                mi.Align ( );
                mi.StartTick ( );
                IC = mi;
                mi.Location = new Point ( 0, 20 );
                Controls.Add ( mi );
                Show ( );
            }
            if ( cls is Vivid3D.Scene.GraphEntity3D || cls is Vivid3D.Terrain.GraphTerrain )
            {
                Vivid3D.Scene.GraphEntity3D cls2 = cls as Vivid3D.Scene.GraphEntity3D;
                Text = "Inspecting:" + cls2.Name + "(3D Entity)";
                if ( IC != null )
                {
                    Controls.Remove ( IC );
                }
                Inspectors.InspectorEntity ei = new Inspectors.InspectorEntity ( );
                Inspecting = ei;
                ei.Entity = cls2;
                ei.Align ( );
                ei.StartTick ( );
                IC = ei;
                ei.Location = new Point ( 0, 20 );
                Controls.Add ( ei );
                Show ( );
            }
            if ( cls is Vivid3D.Lighting.GraphLight3D )
            {
                Vivid3D.Lighting.GraphLight3D cl = cls as Vivid3D.Lighting.GraphLight3D;
                Text = "Inspecting:" + cl.Name + "(3D Light)";

                if ( IC != null )
                {
                    Controls.Remove ( IC );
                }
                Inspectors.InspectorLightControl li = new Inspectors.InspectorLightControl ( );
                Inspecting = li;
                li.Light = cls as Vivid3D.Lighting.GraphLight3D;
                li.Align ( );
                IC = li;
                li.Location = new Point ( 0, 20 );
                Controls.Add ( li );
                Show ( );
            }
            IC.SetUI ( );
        }

        private void Boxx_TextChanged ( object sender, EventArgs e )
        {
            throw new NotImplementedException ( );
        }

        private void inspectBox_CheckedChanged ( object sender, EventArgs e )
        {
            //
            /*
            if (inspectBox.Checked)
            {
                Console.WriteLine("Changed True");
                VividEdit.VividED.Main.BeginInspect();
            }
            else
            // Console.WriteLine("Changed false");
            {
                VividEdit.VividED.Main.EndInspect();
            }
            */
        }

        private void inspectBox_CheckStateChanged ( object sender, EventArgs e )
        {
        }
    }
}