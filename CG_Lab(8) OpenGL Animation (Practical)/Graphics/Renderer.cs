using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;

//include GLM library
using GlmNet;


using System.IO;
using System.Diagnostics;

namespace Graphics
{
    class Renderer
    {
        Shader sh;
        
        uint triangleBufferID;
        uint xyzAxesBufferID;

        //3D Drawing
        mat4 ModelMatrix;
        mat4 ViewMatrix;
        mat4 ProjectionMatrix;
        
        int ShaderModelMatrixID;
        int ShaderViewMatrixID;
        int ShaderProjectionMatrixID;

        const float rotationSpeed = 1f;
        float rotationAngle = 0;

        public float translationX=0, 
                     translationY=0, 
                     translationZ=0;

        Stopwatch timer = Stopwatch.StartNew();

        vec3 triangleCenter;

        public void Initialize()
        {
            string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            sh = new Shader(projectPath + "\\Shaders\\SimpleVertexShader.vertexshader", projectPath + "\\Shaders\\SimpleFragmentShader.fragmentshader");

            Gl.glClearColor(1, 1, 1, 1);
            
            float[] triangleVertices= { 
		         
                 //1
                -2f,2f,0,  //0
                0.671f, 0.58f, 0.322f,
                -4f,3.5f,0,  //1
                0.671f, 0.58f, 0.322f,
                -2.1f,4.6f,0,  //2
                0.671f, 0.58f, 0.322f,  //done
                //2
                -2f,2f,0,  //3
                 0.9f, 0.906f, 0.737f,
                -2.1f,4.6f,0,  //4
                 0.9f, 0.906f, 0.737f,
                1.2f,4.2f,0,  //5
                 0.9f, 0.906f, 0.737f,  //done
                //3
                 -2f,2f,0,  //6
                 0.9f, 0.906f, 0.737f,
                  1.2f,4.2f,0,  //7
                   0.9f, 0.906f, 0.737f,
                  2.5f,-0.5f,0,  //8
                   0.9f, 0.906f, 0.737f, //done
                //4
                 -2f,2f,0,  //9
                  0.9f, 0.906f, 0.737f,
                  2.5f,-0.5f,0,  //10
                   0.9f, 0.906f, 0.737f,
                  2.3f,-4.9f,0,  //11
                   0.9f, 0.906f, 0.737f, //done
                //5
                 2f,2f,0,  //12
                  0.9f, 0.906f, 0.737f,
                 2.3f,-4.9f,0,  //13
                  0.9f, 0.906f, 0.737f,
                 -1.8f,-5.5f,0,  //14
                  0.9f, 0.906f, 0.737f, //done
                //6
                  1.2f,4.2f,0,  //15
                   0.9f, 0.906f, 0.737f,
                  2.5f,-0.5f,0,  //16
                   0.9f, 0.906f, 0.737f,
                  4.8f,-0.5f,0,  //17
                   0.9f, 0.906f, 0.737f, //done
                //7
                   4.8f,-0.5f,0,  //18
                    0.9f, 0.906f, 0.737f,
                   1.2f,4.2f,0,  //19
                    0.9f, 0.906f, 0.737f,
                   8.7f,1.5f,0,  //20
                    0.9f, 0.906f, 0.737f, //done
                //8
                  2.5f,-0.5f,0,  //21
                   0.9f, 0.906f, 0.737f,
                  2.3f,-4.9f,0,  //22
                   0.9f, 0.906f, 0.737f,
                  5.8f,-3.9f,0,  //23
                   0.9f, 0.906f, 0.737f, //done
                  //9
                  4.8f,-0.5f,0,  //24
                  0.9f, 0.906f, 0.737f,
                  5.8f,-3.9f,0,  //25
                   0.9f, 0.906f, 0.737f,//done


              
                   //tail
                //10
                6.5f,0.2f,0,   //27
                 0.9f, 0.906f, 0.737f,
                8f,-1f,0,    //28
                 0.9f, 0.906f, 0.737f,
                5.2f,-2f,0,   //29
                 0.9f, 0.906f, 0.737f, //done
                 
                //11
                5.2f,-2f,0,   //30
                 0.9f, 0.906f, 0.737f,
                8f,-3.8f,0,  //31
                 0.9f, 0.906f, 0.737f,
                5.8f,-3.9f,0,  //32
                 0.9f, 0.906f, 0.737f,//done
                
                 //12
                5.2f,-2f,0,   //33
                 0.9f, 0.906f, 0.737f,
                8.8f,-2.8f,0,  //34
                 0.9f, 0.906f, 0.737f,
                8f,-1f,0,    //35
                 0.9f, 0.906f, 0.737f,//done
                

                //13
                 6.5f,0.2f,0,   //36
                  0.9f, 0.906f, 0.737f,
                 8.8f,0,0,       //37
                  0.9f, 0.906f, 0.737f,
                 7.8f,0.9f,0,   //38
                  0.9f, 0.906f, 0.737f,//done

                  //body

                 //14
                  -4f,3.5f,0,  //39
                  0.671f, 0.58f, 0.322f,
                  -2f,2f,0,  //40
                 0.671f, 0.58f, 0.322f,
                  -5.7f,1.5f,0,  //41
                  0.671f, 0.58f, 0.322f,//done

                 //15
                  -1.8f,-5.5f,0,  //42
                   0.671f, 0.58f, 0.322f,
                  -5.7f,1.5f,0,  //43
                   0.671f, 0.58f, 0.322f,//done

                  //16
                  -1.8f,-5.5f,0,  //44
                  0.671f, 0.58f, 0.322f,
                  -5.5f,-4f,0,  //45
                 0.671f, 0.58f, 0.322f,
                  -5.7f,1.5f,0,  //46
                0.671f, 0.58f, 0.322f,//done

                  //17
                  -5.5f,-4f,0,  //47
                   0.671f, 0.58f, 0.322f,
                  -7.5f,-1.7f,0,  //48
                  0.671f, 0.58f, 0.322f,
                  -5.7f,1.5f,0,  //49
                 0.671f, 0.58f, 0.322f,//done
                  //18
                  -7.5f,-1.7f,0,  //50
                 0.671f, 0.58f, 0.322f,
                  -7.4f,1.9f,0,    //51
                  0.671f, 0.58f, 0.322f,
                  -5.7f,1.5f,0,  //52
                  0.671f, 0.58f, 0.322f,//done

                  //19
                  -5.7f,1.5f,0,  //53
                  0.671f, 0.58f, 0.322f,
                  -6.5f,4.3f,0,   //54
                   0.671f, 0.58f, 0.322f,
                  -7.4f,1.9f,0,    //55
                  0.671f, 0.58f, 0.322f,//done

                  //20
                   -4f,3.5f,0,  //56
                  0.671f, 0.58f, 0.322f,
                  -5.6f,3.9f,0,  //57
                 0.671f, 0.58f, 0.322f,
                  -5.7f,1.5f,0,//58
                 0.671f, 0.58f, 0.322f,//done

                  //21
                  -6.5f,4.3f,0,   //59
                   0.671f, 0.58f, 0.322f,
                  -5.6f ,3.9f,0,  //60
                  0.671f, 0.58f, 0.322f,//done

                  //22
                  -6.5f,4.3f,0,   //61
                  0.671f, 0.58f, 0.322f,
                  -6f,5.5f,0,   //62
                  0.671f, 0.58f, 0.322f,
                  -5.6f,3.9f,0,  //63
                   0.671f, 0.58f, 0.322f,//done

                  //23
                   -6f,5.5f,0,  //64
                  0.671f, 0.58f, 0.322f,
                   -4f,3.5f,0,   //65
                 0.671f, 0.58f, 0.322f,//done
                   //24
                    -6f,5.5f,0, //66
                     0.671f, 0.58f, 0.322f,
                    -3.5f,5.8f,0, //67
                     0.671f, 0.58f, 0.322f,
                    -4f,3.5f,0,  //68
                     0.671f, 0.58f, 0.322f,//done

                    //25
                    -6f,5.5f,0,   //69
                     0.361f, 0.78f, 0.298f,
                    -6f,8f,0,   //70
                     0.361f, 0.78f, 0.298f,
                    -3.5f,5.8f,0, //71
                     0.361f, 0.78f, 0.298f,//done
                    //26
                     -6f,8f,0,  //72
                     0.361f, 0.78f, 0.298f,
                     -4f,8f,0,   //73
                      0.361f, 0.78f, 0.298f,
                    -3.5f,5.8f,0,   //74
                    0.361f, 0.78f, 0.298f,//done
                    //27
                    -4f,8f,0,   //75
                     0.361f, 0.78f, 0.298f,
                    -5.5f,9.8f,0, //76
                    0.361f, 0.78f, 0.298f,
                    -6f,8f,0, //77
                     0.361f, 0.78f, 0.298f,//done


                     //28
                      -5.5f,9.8f,0, //78
                      0.361f, 0.78f, 0.298f,
                      -7f,8.8f,0, //79
                      0.361f, 0.78f, 0.298f,
                      -6f,8f,0, //80
                       0.361f, 0.78f, 0.298f,//done

                    //29
                     -7f,8.8f,0, //81
                    0.361f, 0.78f, 0.298f,
                     -7.7f,6.8f,0, //82
                   0.361f, 0.78f, 0.298f,
                     -6f,8f,0, //83
                  0.361f, 0.78f, 0.298f,//done
                   
                    //30
                    -7.7f,6.8f,0, //84
                     0.361f, 0.78f, 0.298f,
                    -7f,6.7f,0, //85
                    0.361f, 0.78f, 0.298f,
                    -7f,5.8f,0, //86
                    0.361f, 0.78f, 0.298f,
                    -6f,8f,0, //87
                    0.361f, 0.78f, 0.298f,//done
                    //31
                    -7f,5.8f,0, //88
                    0.361f, 0.78f, 0.298f,
                    -6.7f,5.2f,0,   //89
                   0.361f, 0.78f, 0.298f,
                    -6f,5.5f,0,   //90
                   0.361f, 0.78f, 0.298f,

                     -6f,8f,0, //91
                    0f, 0f, 0f,
                     //32
                     -7.7f,6.8f,0, //92
                     0.988f, 1f, 0.039f,
                     -9.7f,3.8f,0, //93
                     0.988f, 1f, 0.039f,
                     -7f,5.8f,0, //94
                     0.988f, 1f, 0.039f,//done

                     //33
                     -9.7f,3.8f,0, //95
                    0.988f, 1f, 0.039f,
                     -8.9f,3.5f,0, //96
                    0.988f, 1f, 0.039f,
                     -7f,5.8f,0, //97
                    0.988f, 1f, 0.039f,//done

                     //34
                     -8.9f,3.5f,0, //98
                    0.988f, 1f, 0.039f,
                     -6.7f,5.2f,0,   //99
                   0.988f, 1f, 0.039f, //done
                     //35
                     -1.8f,-5.5f,0,  //100
                    0.812f, 0.757f, 0.133f,
                     -1.2f,-7.1f,0,  //101
                    0.812f, 0.757f, 0.133f,
                     -1.2f,-5.4f,0,  //102
                     0.812f, 0.757f, 0.133f,//done
                     //36
                     -1.8f,-5.5f,0,  //103
                      0.812f, 0.757f, 0.133f,
                     -2f,-8f,0,  //104
                      0.812f, 0.757f, 0.133f,
                     -1.3f,-8f,0,  //105
                      0.812f, 0.757f, 0.133f,
                     -1.2f,-7.1f,0,  //106
                     0.812f, 0.757f, 0.133f,//done

                     //37
                     -2f,-8f,0,  //107
                      0.812f, 0.757f, 0.133f,
                     -3.5f,-7.8f,0,  //108
                      0.812f, 0.757f, 0.133f,
                     -3.5f,-8.3f,0,  //109
                      0.812f, 0.757f, 0.133f,//done

                     //38
                     -3.5f,-8.3f,0,  //110
                      0.812f, 0.757f, 0.133f,
                     -4f,-8.7f,0,  //111
                      0.812f, 0.757f, 0.133f,
                     -3.7f,-9.2f,0,  //112
                      0.812f, 0.757f, 0.133f,
                      -2f,-8f,0,  //113
                       0.812f, 0.757f, 0.133f,//done

                      //39
                      -3.7f,-9.2f,0,  //114
                     0.812f, 0.757f, 0.133f,
                      -1.8f,-9.2f,0,  //115
                      0.812f, 0.757f, 0.133f,
                      -2f,-8f,0,  //116
                     0.812f, 0.757f, 0.133f,//done

                      //40
                      -1.8f,-9.2f,0,  //117
                      0.812f, 0.757f, 0.133f,
                      -1.2f,-9f,0,  //118
                     0.812f, 0.757f, 0.133f,
                      -2f,-8f,0,  //119
                     0.812f, 0.757f, 0.133f,

                      //41
                      -1.2f,-9f,0,  //120
                     0.812f, 0.757f, 0.133f,
                     -1.3f,-8f,0,  //121
                   0.812f, 0.757f, 0.133f,

                     -7.8f,5.8f,0,  //122
                    

               


            }; 
            
            triangleCenter = new vec3(0, 0, -6);

            float[] xyzAxesVertices = {
		        //x
		        -100.0f, 0.0f, 0.0f,
                0.0f, 1.0f, 0.0f, 
		        100.0f, 0.0f, 0.0f,
                1.0f, 0.0f, 0.0f, 
		        //y
	            0.0f, -100.0f, 0.0f,
                1.0f,0.0f, 0.0f, 
		        0.0f, 100.0f, 0.0f,
                0.0f, 1.0f, 0.0f, 
		        //z
	            0.0f, 0.0f, 100.0f,
                0.0f, 1.0f, 1.0f,  
		        0.0f, 0.0f, -100.0f,
                0.0f, 1.0f, .0f,  
            };


            triangleBufferID = GPU.GenerateBuffer(triangleVertices);
            xyzAxesBufferID = GPU.GenerateBuffer(xyzAxesVertices);

            // View matrix 
            ViewMatrix = glm.lookAt(
                        new vec3(50, 50, 50), // Camera is at (0,5,5), in World Space
                        new vec3(0, 0, 0), // and looks at the origin
                        new vec3(0, 1, 0)  // Head is up (set to 0,-1,0 to look upside-down)
                );
            // Model Matrix Initialization
            ModelMatrix = new mat4(1);

            //ProjectionMatrix = glm.perspective(FOV, Width / Height, Near, Far);
            ProjectionMatrix = glm.perspective(50.0f, 4.0f / 3.0f, 0.1f, 100.0f);
            
            // Our MVP matrix which is a multiplication of our 3 matrices 
            sh.UseShader();


            //Get a handle for our "MVP" uniform (the holder we created in the vertex shader)
            ShaderModelMatrixID = Gl.glGetUniformLocation(sh.ID, "modelMatrix");
            ShaderViewMatrixID = Gl.glGetUniformLocation(sh.ID, "viewMatrix");
            ShaderProjectionMatrixID = Gl.glGetUniformLocation(sh.ID, "projectionMatrix");

            Gl.glUniformMatrix4fv(ShaderViewMatrixID, 1, Gl.GL_FALSE, ViewMatrix.to_array());
            Gl.glUniformMatrix4fv(ShaderProjectionMatrixID, 1, Gl.GL_FALSE, ProjectionMatrix.to_array());

            timer.Start();
        }

        public void Draw()
        {
            sh.UseShader();
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);

            #region XYZ axis

            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, xyzAxesBufferID);
            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, new mat4(1).to_array()); // Identity

            Gl.glEnableVertexAttribArray(0);
            Gl.glEnableVertexAttribArray(1);

            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)0);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)(3 * sizeof(float)));
             
            Gl.glDrawArrays(Gl.GL_LINES, 0, 12);

            Gl.glDisableVertexAttribArray(0);
            Gl.glDisableVertexAttribArray(1);

            #endregion

            #region Animated Triangle
            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, triangleBufferID);
            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, ModelMatrix.to_array());

            Gl.glEnableVertexAttribArray(0);
            Gl.glEnableVertexAttribArray(1);

            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)0);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)(3 * sizeof(float)));

        
            Gl.glDrawArrays(Gl.GL_POLYGON, 0, 42);

           
            Gl.glDrawArrays(Gl.GL_POLYGON, 3, 6);


            Gl.glDrawArrays(Gl.GL_POLYGON, 6, 9);

            
            Gl.glDrawArrays(Gl.GL_POLYGON, 9, 1);

            
            Gl.glDrawArrays(Gl.GL_POLYGON, 12, 9);

            
            Gl.glDrawArrays(Gl.GL_POLYGON, 15, 3);

        
            Gl.glDrawArrays(Gl.GL_POLYGON, 18, 3);

        
            Gl.glDrawArrays(Gl.GL_POLYGON, 21, 8);

            
            Gl.glDrawArrays(Gl.GL_POLYGON, 24, 5);


            Gl.glDrawArrays(Gl.GL_POLYGON, 29, 9);

           
            Gl.glDrawArrays(Gl.GL_POLYGON, 32, 3);

           
            Gl.glDrawArrays(Gl.GL_POLYGON, 35, 3);

           
            Gl.glDrawArrays(Gl.GL_POLYGON, 38, 5);

            
            Gl.glDrawArrays(Gl.GL_POLYGON, 43, 3);

            
            Gl.glDrawArrays(Gl.GL_POLYGON, 46, 3);

         
            Gl.glDrawArrays(Gl.GL_POLYGON, 49, 3);

           
            Gl.glDrawArrays(Gl.GL_POLYGON, 52, 3);

          
            Gl.glDrawArrays(Gl.GL_POLYGON, 55, 5);

            Gl.glDrawArrays(Gl.GL_POLYGON, 60, 5);


            //// 24
            Gl.glDrawArrays(Gl.GL_POLYGON, 65, 3);

            //// 25
            Gl.glDrawArrays(Gl.GL_POLYGON, 68, 22);

            //eye
            Gl.glPointSize(4);
            Gl.glDrawArrays(Gl.GL_POINTS, 90, 1);

            //// 32
            Gl.glDrawArrays(Gl.GL_POLYGON, 91, 8);

            Gl.glDrawArrays(Gl.GL_POLYGON, 99, 3);

            
            Gl.glDrawArrays(Gl.GL_POLYGON, 102, 4);

            Gl.glDrawArrays(Gl.GL_POLYGON, 106, 15);
            Gl.glPointSize(2);
            Gl.glDrawArrays(Gl.GL_POINTS, 121, 1);


            Gl.glDisableVertexAttribArray(0);
            Gl.glDisableVertexAttribArray(1);
            #endregion
        }
        

        public void Update()
        {

            timer.Stop();
            var deltaTime = timer.ElapsedMilliseconds/1000.0f;

            rotationAngle += deltaTime * rotationSpeed;

            List<mat4> transformations = new List<mat4>();
            transformations.Add(glm.translate(new mat4(1), -1 * triangleCenter));
            transformations.Add(glm.rotate(rotationAngle, new vec3(0, 0, 1)));
            transformations.Add(glm.translate(new mat4(1),  triangleCenter));
            transformations.Add(glm.translate(new mat4(1), new vec3(translationX, translationY, translationZ)));

            ModelMatrix =  MathHelper.MultiplyMatrices(transformations);
            
            timer.Reset();
            timer.Start();
        }
        
        public void CleanUp()
        {
            sh.DestroyShader();
        }
    }
}
