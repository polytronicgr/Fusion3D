#version 440 core

in vec2 UV;
in vec4 col;

out vec4 color;

uniform float blur;
uniform float refract;
uniform sampler2D tR;
uniform sampler2D tB;
uniform sampler2D tN;
uniform bool refractOn;

void main(){
    vec2 nv = UV;
    nv.y = 1.0 - nv.y;
    
    vec3 fc = vec3(0,0,0);

    float bf = blur * 0.06;
    float rf = refract * 0.2;

    int x,y;
    x=0;
    y=0;
    int cc = 0;

  if(refractOn){
                vec2 no = texture2D(tN,nv).xy;
                no.x = -1 + no.x*2;
                no.y = -1 + no.y*2;
                nv.x = nv.x - no.x * rf;
                nv.y = nv.y - no.y * rf;
                if(nv.x<0) nv.x = 0;
                if(nv.y<0) nv.y = 0;
                if(nv.x>1) nv.x = 1;
                if(nv.y>1) nv.y = 1;
            }

    for(x=-3;x<3;x++)
    {
        for(y=-3;y<3;y++)
        {
            vec2 buv = nv;
            buv.x = buv.x + (x*bf);
            buv.y = buv.y + (y*bf);
          
            if(buv.x<0 || buv.x>1 || buv.y<0 || buv.y>1){
            }else{
            fc = fc + texture2D(tB,buv).rgb;
            cc++;
            }
        }
    }
    fc = fc / cc;
    //vec4 co = texture(tB,nv);
    vec4 icol = texture2D(tR,UV) * col;
    icol.a = col.a * icol.a;
    icol.rgb =fc*0.5 + icol.rgb*0.25;
    color = icol;
}