# WebRanker
Accurately rank lists of arbitrary size from your most favorite to least favorite

This is a .NET adaption of my python app called "Ranker". That can be found
here: https://bitbucket.org/TheFrozenMawile/ranker/src/master/

The way this program works is simple and mathematically accurate. The user inputs
a creates a list of anything they want - colors, songs, movies. Next, they are
given "matchups" - every possible 2-item combination of items from the list.
The user chooses which of the two is their favorite. After all matchups have
been exhausted, their list is sorted according to the number of times each item
was voted for. Because of the way that combinations work, it's impossible for
any two items to have the same number of votes. This provides the user with a
sort of "tier list", with every item ranked from favorite to least favorite.

It's recommended that this is done with fairly small lists, as the formala for
finding number of matchups (n choose 2) grows large very quickly due to factorials.
A list with 6 items (6 choose 2) would have only 15 matchups, but a list with
20 items has 190 matchups. The exact formula for finding the number of matchups
is (n!)/2(n - 2)! where n is the number of items in the list.
