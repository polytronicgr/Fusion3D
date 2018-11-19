module test

	func test()
	
	end
	
	func genString()
	
		return "Hey!";
	
	end

end


func test()

	tc = new test();
	
	printf("Out:"+tc.genString());

	return 5;

end