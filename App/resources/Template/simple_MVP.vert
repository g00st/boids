#version 400 core

layout(location = 0) in vec2 inPosition;
uniform mat4 u_MVP;
uniform mat4 u_Model;
uniform mat4 u_View;
uniform mat4 u_Projection;
void main()
{
    vec4 worldPosition = u_Model * vec4(inPosition, 0.0, 1.0);
    vec4 cameraPosition = u_View * worldPosition;
    gl_Position = u_Projection * cameraPosition;
}