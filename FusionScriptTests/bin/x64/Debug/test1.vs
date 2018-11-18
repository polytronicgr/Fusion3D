module other

	var c = 3;

end

module draw

	var a = 2;
	var l = new other();

	func test()
	
		a=5;
	
	end

end

func test()

	ant = new draw();

	printf("Test:"+ant.l.c);

end