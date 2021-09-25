# MultiValueDictionary #
.NET 5 command line application that stores a multivalue dictionary in memory.

### Commands ###

General pattern is [command] _[key]_ _[member]_. Command names are not case-sensitive, but keys and members are. Some commands expect no parameters. Some expect a key only, Some expect both a key and a member.

#### Add ####
Adds a member to a given key. Requires key and member parameters.
_Example:_ `add key member`

#### AllMembers ####
Lists all members for all keys. No parameters.
_Example:_ `allmembers`

#### Clear ####
Clears all keys and members from the Dictionary. No parameters.
_Example:_ `clear`

#### Items ####
Lists all keys and associated members in the dictionary. No parameters.
_Example:_ `items`

#### KeyExists ####
Indicates if a given key exists in the dictionary. Requires key parameter.
_Example:_ `keyexists key`

#### Keys ####
Lists all keys in the dictionary. No Parameters.
_Example:_ `keys`

#### MemberExists ####
Indicates if a given member exists for a given key. Requires key and member parameters.
_Example:_ `memberexists key member`

#### Members ####
Lists all members for a given key. Requires key parameter.
_Example:_ `members key`

#### Remove ####
Removes a given member from a given key. Requires key and member parameters.
_Example:_ `remove key member`

#### RemoveAll ####
Removes a given key, along with all its members. Requires key parameter.
_Example:_ `removeall key`