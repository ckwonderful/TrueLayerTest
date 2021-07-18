#Running program

- go to directory: TrueLayerTest.Api\TrueLayerTest.Api
- dotnet run
- get postman
- create GET with http://localhost:5000/pokemon/{name}


#In Production I would
- create things in config
- do some logging including requets/responses
- create microservice for the translation
- exception handling 
- more unit tests, around api etc, plus not just happy path, just did minumum for the purpose of the test
- integration testing
- retry mechanisms

Full disclosure - I had a busy weekend, therefore I started this Friday evening but was tired, so then I was going to see my dad Satruday so tried to fit in a bit before I left, and today I've been doing it on and off today while I've been trying to paint my flat for it being photographed to put on the market. Point I'm making and that's why it's been a bit disjointed in the commits plus I should really have re-read the requirements, hence the sort of fudge at the end with the translation boolean. I really don't have any more time so hopefully it's good enough, but if not c'est la vie.

Also I haven't used .net outside of windows, hopefully kestrel is grand.