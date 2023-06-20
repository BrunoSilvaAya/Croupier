# Croupier
API that serves a deck of cards

The idea to build this came from a simple question I saw on Twitter: "Is there a public API that serves cards from a playing deck?".

I didn't know one already existed (and you should definitely check out www.deckofcardsapi.com), but I felt like it would be a fun little project to build on in C#

And what a great chance to make us of minimal APIs and C# 10 features and capabilities

Thanks for stopping by and I hope you like it!

# How to play
Send a GET request at the /new-game endpoint. It'll create a session with a 52 cards deck and give you a sessionId. 

Pass that sessionId to the /see-deck endpoint to see your entire deck. It'll be ordered from King of Clubs (top of the pile) to Ace of Spades (bottom of the pile)

You can /shuffle or /draw-card using the sessionId as well. When you're out of cards, calling /draw-card will give an empty return.
