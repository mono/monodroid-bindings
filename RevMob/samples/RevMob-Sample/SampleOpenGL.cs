using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Opengl;
using Java.Nio;
using Javax.Microedition.Khronos.Opengles;
using Com.Revmob;

namespace RevMobSample
{
    [Activity(ConfigurationChanges = Android.Content.PM.ConfigChanges.KeyboardHidden | Android.Content.PM.ConfigChanges.Orientation)]			
    public class SampleOpenGL : SampleApp
    {
        class SampleRenderer : Java.Lang.Object, GLSurfaceView.IRenderer {
            private Cube mCube = new Cube();
            private float mCubeRotation;

          
            public void OnSurfaceCreated(Javax.Microedition.Khronos.Opengles.IGL10 gl, Javax.Microedition.Khronos.Egl.EGLConfig config) {
                gl.GlClearColor(0.0f, 0.0f, 0.0f, 0.5f);

                gl.GlClearDepthf(1.0f);
                gl.GlEnable(GL10.GlDepthTest);
                gl.GlDepthFunc(GL10.GlLequal);

                gl.GlHint(GL10.GlPerspectiveCorrectionHint, GL10.GlNicest);

            }


            public void OnDrawFrame(Javax.Microedition.Khronos.Opengles.IGL10 gl) {
                gl.GlClear(GL10.GlColorBufferBit | GL10.GlDepthBufferBit);
                gl.GlLoadIdentity();

                gl.GlTranslatef(0.0f, 0.0f, -10.0f);
                gl.GlRotatef(mCubeRotation, 1.0f, 1.0f, 1.0f);

                mCube.draw(gl);

                gl.GlLoadIdentity();

                mCubeRotation -= 1.0f;
            }


            public void OnSurfaceChanged(Javax.Microedition.Khronos.Opengles.IGL10 gl, int width, int height) {
                gl.GlViewport(0, 0, width, height);
                gl.GlMatrixMode(GL10.GlProjection);
                gl.GlLoadIdentity();
                GLU.GluPerspective(gl, 45.0f, (float)width / (float)height, 0.1f, 100.0f);
                gl.GlViewport(0, 0, width, height);

                gl.GlMatrixMode(GL10.GlModelview);
                gl.GlLoadIdentity();
            }
        }

        class SampleGLSurfaceView : GLSurfaceView {
            Activity activity;
            SampleRenderer mRenderer;

            public SampleGLSurfaceView(Activity activity) : base(activity) {
           
                this.activity = activity;
                mRenderer = new SampleRenderer();
                SetRenderer(mRenderer);
            }

            public override bool OnTouchEvent(MotionEvent evt) {
                QueueEvent(new Java.Lang.Runnable(delegate {
                    switch(evt.Action)
                    {
                        case MotionEventActions.Up:
                            RevMob revmob = RevMob.Session();
                            revmob.ShowFullscreen(this.activity);
                            break;
                    }
                }));
                /*
                queueEvent(new Runnable() {
                    @Override
                    public void run() {
                        switch(evt.getAction()) {
                            case MotionEvent.ACTION_UP:
                                RevMob.session().showFullscreen(SampleGLSurfaceView.this.activity);
                                break;
                        }
                    }});
                    */
                return true;
            }
        }

        class Cube {
            private FloatBuffer mVertexBuffer;
            private FloatBuffer mColorBuffer;
            private ByteBuffer  mIndexBuffer;

            private float [] vertices = {
                -1.0f, -1.0f, -1.0f,
                1.0f, -1.0f, -1.0f,
                1.0f,  1.0f, -1.0f,
                -1.0f, 1.0f, -1.0f,
                -1.0f, -1.0f,  1.0f,
                1.0f, -1.0f,  1.0f,
                1.0f,  1.0f,  1.0f,
                -1.0f,  1.0f,  1.0f
            };
            private float  [] colors = {
                0.0f,  1.0f,  0.0f,  1.0f,
                0.0f,  1.0f,  0.0f,  1.0f,
                1.0f,  0.5f,  0.0f,  1.0f,
                1.0f,  0.5f,  0.0f,  1.0f,
                1.0f,  0.0f,  0.0f,  1.0f,
                1.0f,  0.0f,  0.0f,  1.0f,
                0.0f,  0.0f,  1.0f,  1.0f,
                1.0f,  0.0f,  1.0f,  1.0f
            };

            private byte [] indices = {
                0, 4, 5, 0, 5, 1,
                1, 5, 6, 1, 6, 2,
                2, 6, 7, 2, 7, 3,
                3, 7, 4, 3, 4, 0,
                4, 7, 6, 4, 6, 5,
                3, 0, 1, 3, 1, 2
            };

            public Cube() {
                ByteBuffer byteBuf = ByteBuffer.AllocateDirect(vertices.Length * 4);
                byteBuf.Order(ByteOrder.NativeOrder());
                mVertexBuffer = byteBuf.AsFloatBuffer();
                mVertexBuffer.Put(vertices);
                mVertexBuffer.Position(0);

                byteBuf = ByteBuffer.AllocateDirect(colors.Length * 4);
                byteBuf.Order(ByteOrder.NativeOrder());
                mColorBuffer = byteBuf.AsFloatBuffer();
                mColorBuffer.Put(colors);
                mColorBuffer.Position(0);

                mIndexBuffer = ByteBuffer.AllocateDirect(indices.Length);
                mIndexBuffer.Put(indices);
                mIndexBuffer.Position(0);
            }

            public void draw(Javax.Microedition.Khronos.Opengles.IGL10 gl) {
                gl.GlFrontFace(GL10.GlCw);

                gl.GlVertexPointer(3, GL10.GlFloat, 0, mVertexBuffer);
                gl.GlColorPointer(4, GL10.GlFloat, 0, mColorBuffer);

                gl.GlEnableClientState(GL10.GlVertexArray);
                gl.GlEnableClientState(GL10.GlColorArray);

                gl.GlDrawElements(GL10.GlTriangles, 36, GL10.GlUnsignedByte, mIndexBuffer);

                gl.GlDisableClientState(GL10.GlVertexArray);
                gl.GlDisableClientState(GL10.GlColorArray);
            }
        }

        SampleGLSurfaceView glSurfaceView;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
            glSurfaceView = new SampleGLSurfaceView(this);
            SetContentView(glSurfaceView);
        }

        protected override void OnPause()
        {
            base.OnPause();
            glSurfaceView.OnPause();
        }

        protected override void OnResume()
        {
            base.OnResume();
            glSurfaceView.OnResume();
        }
    }
}

