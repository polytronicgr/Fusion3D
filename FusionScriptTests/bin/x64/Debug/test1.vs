module other

	var c = 3;

end

module draw

	var a = 2;
	var l = new other();

	func test(time)
	
		return 30+time;
	
	end

end

func test()

	ant = new draw();


	printf("Test:"+ant.test(20));

	return 5;

end