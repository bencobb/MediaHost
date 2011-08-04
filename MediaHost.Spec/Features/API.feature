Feature: API Interaction
	In order to access Media Host
	As an API user
	I need the available API calls to function

Scenario: Add Blank Entity
	Given I have an empty entity 
	Then I call AddEntity 
		And I should receive error message: 
		| Exception                          |
		| Exception: Entity Name is Required |

Scenario: Add Entity
	Given I have a valid entity
	Then I call AddEntity 
		And the entity should be added to the db and return with an Id
		
Scenario: Add Blank Group
	Given I have an empty group 
	Then I call AddGroup
		And I should receive error message: 
		| Exception                      |
		| Exception: EntityId Required   |
		| Exception: Group Name Required |

Scenario: Add Group
	Given I have a valid group
	Then I call AddGroup
		And the group should be added to the db and return with an Id

Scenario: Add Blank Playlist
	Given I have an empty playlist
	Then I call AddPlaylist
		And I should receive error message: 
		| Exception                         |
		| Exception: EntityId Required      |
		| Exception: Playlist Name Required |

Scenario: Add Playlist
	Given I have a valid playlist
	Then I call AddPlaylist
		And the playlist should be added to the db and return with an Id

Scenario: Add Blank File
	Given I have an empty file
	Then I call AddFile
		And I should receive error message: 
		| Exception                       |
		| Exception: EntityId Required    |
		| Exception: File Name Required   |
		| Exception: File Upload Required |

Scenario: Add File
	Given I have a valid file
	Then I call AddFile
		And the file should be added to the db and return with an Id
		And the file should be added to the IO system

Scenario: Test
	Given I have an empty 
	When jlkjl
		And lkjlkjlk
	Then lkjlkj
		And lkjlkdsdf
