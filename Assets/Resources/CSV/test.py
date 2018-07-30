import ConfigParser  
  
config = ConfigParser.ConfigParser()  
config.read("test.conf")  
  
def get_foo():  
    return config.get("locations", "foo")  
  
def get_bar():  
    return config.get("locations", "bar")