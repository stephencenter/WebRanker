# WebRanker
Accurately rank lists of arbitrary size from your most favorite to least favorite

This is a .NET adaption of my python app called "Ranker". That can be found
here: https://bitbucket.org/TheFrozenMawile/ranker/src/master/

The way this program works is simple and mathematically accurate. The user inputs
a creates a list of anything they want - colors, songs, movies. Next, they are
given some matchups - every possible 2-item combination of items from the list.
The user chooses which of the two is their favorite. After all matchups have
been exhausted, their list is sorted according to the number of times each item
was voted for. Because of the way that combinations work, it's impossible for
any two items to have the same number of votes. This provides the user with a
sort of "tier list", with every item ranked from favorite to least favorite.

Example list:

# Favorite Season

Spring

Summer

Fall

Winter

Example matchups:

Spring vs Summer -> Spring

Spring vs Fall -> Fall

Spring vs Winter -> Spring

Summer vs Fall -> Fall

Summer vs Winter -> Winter

Fall vs Winter -> Fall


Example Rankings:

Fall [Voted for 3 times]

Spring [Voted for 2 times]

Winter [Voted for 1 time]

Summer [Voted for 0 times]