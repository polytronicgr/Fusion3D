module other

	var num = 55;
	
	func change()
	
		num =25;
	
	
	end
	

end

module draw

	var c1 = new other();
	var val = 20;

	func rect(x,y,w,h)
	
		printf("Rect:"+x);
	
	end

end

func testFunc()


	drawI = new draw();
	
	drawI.rect(20,20,200,200);
	

end