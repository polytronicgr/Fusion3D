computestruct display = byte*;

computestruct rect

	int x;
	int y;
	int w;
	int h;
	
end

compute imageRender

	rect draw in;
	display dis out one;
	
	func void drawRect(rect r1)
	
		int dx;
		int dy;
	
		for(int dy = r1.y;dy<(r1.y+r1.h);dy=dy+1)
		
			for(dx=r1.x;dx<(r1.x+r1.w);dx=dx+1)
			
					int loc = (dy * r1.w) * 3;
					loc = loc + (dx * 3);
			
			end
		
		end
		
	
	end
	

end


func Entry()

	printf("Welcome to Foom! - A doom-clone wrote in Fusion");

	InitFoom();
	

end