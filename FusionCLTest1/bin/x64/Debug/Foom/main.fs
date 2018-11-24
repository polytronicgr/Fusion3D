computestruct display

	int width;
	int height;
	byte* rgb;

end

computestruct rect

	int x;
	int y;
	int w;
	int h;
	
end

compute imageRender

	rect draw in;
	display dis1 out one;
	
	func void drawRect(rect do)
	
		int dx;
		int dy;
	
		for(int dy = do.y;dy<(do.y+do.h);dy=dy+1)
		
			for(dx=do.x;dx<(do.x+do.w);dx=dx+1)
			
					int loc = (dy * do.w) * 3;
					loc = loc + (dx * 3);
			
			end
		
		end
		
	
	end
	

end


func Entry()

	printf("Welcome to Foom! - A doom-clone wrote in Fusion");

	InitFoom();
	

end