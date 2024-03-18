#version 400 core

layout(location = 0) in vec2 inPosition;
layout(location = 1) in vec4 instancePosition; // Using vec3 for world 

out vec2 textureCords;

uniform mat4 u_Model;
uniform mat4 u_View;
uniform mat4 u_Projection;

void main()
{
    textureCords = inPosition;
    float theta = instancePosition.z; // rotation in radians
    mat2 rotationMatrix = mat2(cos(theta), -sin(theta), sin(theta), cos(theta));
    vec2 rotatedPosition = rotationMatrix * (inPosition-0.5); // rotate around the center
    vec4 worldPosition = u_Model * vec4(rotatedPosition+ 0.5, 0.0, 1.0);
    worldPosition.xy += instancePosition.xy;
    vec4 cameraPosition = u_View * worldPosition;
    gl_Position = u_Projection * cameraPosition;
}
