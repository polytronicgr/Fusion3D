module draw

	func draw()
	
		printf("InitDraw");
	
	end

	func testMeth()
	
		printf("TestMeth!");
	
	end

end

func test()

	val = new draw();

	val.testMeth();

end