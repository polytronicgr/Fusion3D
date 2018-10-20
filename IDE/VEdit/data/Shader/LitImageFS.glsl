uniform sampler2D tDiffuse;

uniform vec3 lPos;
uniform vec3 lDif;
uniform vec3 lSpec;
uniform float lShiny;
uniform float lRange;

uniform float sWidth;
uniform float sHeight;

void main(){

    vec4 tc = texture2D(tDiffuse,gl_TexCoord[0].st);

    vec2 pos = gl_FragCoord.xy;
   
    pos.y = sHeight-pos.y;

    vec2 lp = vec2(lPos.x,lPos.y);

    float xd = lp.x-pos.x;
    float yd = lp.y-pos.y;

    float dis = sqrt(xd*xd+yd*yd);

    dis = dis / lRange;


    if(dis>1.0)
    {

        dis = 1.0;

    }
    
    dis = 1.0-dis;


     tc.xyz = tc.xyz * lDif * dis;

    


    gl_FragColor = tc;

}