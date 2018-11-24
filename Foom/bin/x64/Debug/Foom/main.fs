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
	
	func void drawRect(int x,int y,int w,int h,rect do)
	
		int dx;
		int dy;
	
		for(int dy = y;dy<(y+h);dy=dy+1)
		
			for(dx=x;dx<(x+w);dx=dx+1)
			
				
			
			end
		
		end
		
	
	end
	

end


func Entry()

	printf("Welcome to Foom! - A doom-clone wrote in Fusion");

	InitFoom();
	

end