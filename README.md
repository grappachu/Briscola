# Briscola

A simple game for "Italian Briscola" made with a TDD approach


## Contributing

Even if project is intended for fun with my office-mates, every contribute or suggestion is welcome.

### How to

There are three ways to contribute:

#### 1. By Fork

Fork repository into your GitHub account, then create your strategy and make a pull request when ready

#### 2. As Contributor

Send me a message and i will add your account to contributors

#### 3. Offline

Clone repo, write your code, send me files and I will do the merge


Then create your own strategy branch naming it as 



### Coding Rules

1. Create a folder for your strategy in project

	    Grappachu.Briscola.Strategies

	The name should looks like 

		feature/[surname]+[first-letter-of-name]


2. Build your strategy by extending class StrategyBase
3. Give it a great name (best if ascii lowercase)
4. Register your strategy in the `StrategyFactory.cs` (possibly with the name you provided previously)

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