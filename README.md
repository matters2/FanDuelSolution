# :football: FanDuel Depth Charts API

## Overview

The NFL Depth Charts API is a .NET Core 8 Web API designed to manage and work with NFL team depth charts. It provides functionality to add players to depth charts, remove players from depth charts, retrieve backups for a given player and position, and get the full depth chart for a team.

## Data Source

The data for this API is sourced from [OurLads](https://www.ourlads.com/nfldepthcharts), a website that provides NFL depth chart information. The data may change based on roster moves made by teams throughout the NFL season.

## Data Model

The data model for players in the API generally follows this structure:
    
    {
      "number": 12,
      "name": "Tom Brady",
      "position": "QB"
    }

Note: Players can be listed on the depth chart for multiple positions.

## Use Cases

### AddPlayerToDepthChart (POST)

- Adds a player to the depth chart at a given position.
- If no position depth is specified, the player is added to the end of the depth chart at that position.
- Existing players below the added player are moved down a position depth.

### RemovePlayerFromDepthChart (POST)

- Removes a player from the depth chart for a given position.
- Returns the removed player.
- Returns an empty list if the player is not listed in the depth chart at that position.

### GetBackups (GET)

- Retrieves all backup players for a given player and position.
- Returns an empty list if the given player has no backups.
- Returns an empty list if the given player is not listed in the depth chart at that position.

### GetFullDepthChart (GET)

- Prints out the full depth chart with every position on the team and every player within the depth chart.

## Assumptions

- No application state required (no CRUD operations on hosted database available), using In Memory solution for the life of the application instance
- No Front-end application to integrate with the API

## Technology Used

- Framework: .NET Core 8
- API Controller Actions: 4
- Testing Framework: xUnit
- Testing Libraries: AutoFixture, Mock, Fluent Assertions
- API Documentation: Swagger
- Launch Configuration: Configured to navigate to Swagger page on launch for easy use.
  
## Getting Started

To start using the NFL Depth Charts API, follow these steps:

- Clone the repository from GitHub.
- Build and run the project.
- Navigate to the Swagger page to explore and interact with the API endpoints (landing page by default).
- Start using the API to manage and analyze NFL team depth charts.

## Developer

- **Name**: Matt Stacy
- **GitHub**: [matters2](https://github.com/matters2)
