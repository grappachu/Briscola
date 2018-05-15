# Briscola

A simple game for "Italian Briscola" made with a TDD approach


## Contributing

Even if project is intended for fun with my office-mates, every contribute or suggestion is welcome.

###How to

First clone the repository

	git clone https://github.com/grappachu/Briscola.git

and initialize git flow

	git flow init

Then start your feature for your strategy naming it as 

`surname`+`first-letter-of-name`

so the command will be like

	git flow feature start prenassid

**You are now ready to start coding!**

1. Create a folder for your strategy named as the feature in project

	    Grappachu.Briscola.Strategies

2. Build your strategy by extending class StrategyBase
3. Give it a great name (best if ascii lowercase)
4. Register your strategy in the `StrategyFactory.cs` possibly with the name you provided previously

	 	{"awesome-player", () => new AwesomeStrategy()},
	   
Make your code awesome and more awesome but mind these rules

- **All code must be wrote into your folder**
- **Your strategy must not watch other players hand-cards**
- **Your strategy must not access decks (except yours)**
	- But you are allowed to use Look method to track all played cards
- **Your strategy must not change assigned cards in any way**
- **Your strategy must not make system crash or other players crash, of course**


When ready push your feature Your strategy will be reviewed and if ok, merged.

 **On first Friday of every month the strategies will play a great tournament and the winner will be eligible of honour and glory!**

GG. 