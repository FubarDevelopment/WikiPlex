version = File.read(File.expand_path("../VERSION",__FILE__)).strip

Gem::Specification.new do |spec|
	spec.platform	       = Gem::Platform::RUBY
	spec.name		       = 'wikiplex'
	spec.version	       = version
	spec.files 		       = Dir['lib/**/*'] + Dir['docs/**/*']
	
	spec.summary	       = 'The CodePlex wiki engine for use in .NET applications'
	spec.description       = <<-EOF
		WikiPlex is a regular expression based wiki engine that allows developers 
		to integrate a wiki experience into an existing .NET application seamlessly 
		and with little effort. Built and used by the CodePlex team, WikiPlex has 
		been thoroughly tested in real-world scenarios!
	EOF
	
	spec.authors		   = 'Matt Hawley'
	spec.email		       = 'N/A'
	spec.homepage	 	   = 'http://wikiplex.codeplex.com'
	spec.rubyforge_project = 'wikiplex'
end