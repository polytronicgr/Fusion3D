module other

	var num = 55;
	
	func change()
	
		num =25;
	
	end
	

end

module draw

	var c1 = new other();
	var val = 20;

	func static rect(x,y,w,h)
	
		
	
	end

end

func testFunc()

	test = new draw();
	test2 = new other();
	
	printf("Testing:"+test.c1.num);
	printf("Test2:"+test2.num);
	

end