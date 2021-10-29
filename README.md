# NWPriceTracker
New World Price Tracker is a self-hosted web platformwhich allows you to keep track of prices in all trading zones for the game New World.  
It can help you determine where to buy and sell your items, or assist with arbitrage trading decisions.

This application was made for personal use for me and my friends, but there are instructions below on how you can set up your own instance of the application.

### Preview
[![New World Price Tracker preview](https://user-images.githubusercontent.com/9306304/139482301-9476db8b-a59e-4705-81ab-8f30f1537023.png)](https://user-images.githubusercontent.com/9306304/139482100-38e6bfac-ddba-46a2-9d2e-0558622e7cea.png)

### Features:
- Update prices for each item in each trading zone
- Private access or share access with friend group using Discord login
- Mark specific items as favorite for easy access
- Track when prices were updated
- Live price updates between everyone online
- Everyone can invite friends to join!

### FAQ
**Does this tool conform with the terms and conditions?**  
Yes. This tool does not interact with the game in any way.
   
### Installation
1. Install PostgreSQL and create a database and postgres user for NWPT
2. Create a unix user to host the application and log in as that user  
    ```
    sudo useradd -m nwpt
    su nwpt
    ```
3. Clone and compile the application
    ```
    cd ~
    git clone https://github.com/NotCoffee418/NWPriceTracker.git
    cd NWPriceTracker/NWPriceTracker/Server
    dotnet publish "NWPriceTracker.Server.csproj" -c Release -o ~/publish
    ```
4. Prepare appsettings.json
   ```
   cd ~/publish
   cp appsettings.example.json appsettings.json
   nano appsettings.json
   ```
5. Change the connection string using your own information in this format:
    ```
    "Server=127.0.0.1;Port=5432;Database=NWPriceTracker.Production;Userid=NWPriceTracker;Password=PASSWORDHERE"
    ``` 
6. CTRL+O to save and CTRL-X to exit out of nano
7. Create an application [here](https://discord.com/developers/applications/) and note the Client ID and Client Secret.
8. Setup OAuth and grant access to one user
   1. Run the application once to generate your database.
    ```
    cd ~/publish
    dotnet NWPriceTracker.Server.dll
    ```
   2. Connect to your database manually  
   `psql -d DATABASENAME -U DATABASEUSER -W`
   1. Run the following queries, with your information
   ```
    UPDATE setting SET settingvalue = 'DISCORDOAUTHCLIENTIDHERE' WHERE settingkey = 'DiscordClientId';
    UPDATE setting SET settingvalue = 'DISCORDOAUTHSECRETHERE' WHERE settingkey = 'DiscordClientSecret';
    INSERT INTO account (discordhandle, profilepictureurl) VALUES ('YOURDISCORDHANDLE#1234', '/img/unknown-user.png');
   ```
9. Run the application
    ```
    cd ~/publish
    dotnet NWPriceTracker.Server.dll
    ```
10. Instaling the item database
    1. Navigate to https://YOURSERVERIP:9005/install
    2. Click the Install button and wait for all items to be imported 
11. Start using the application through https://YOURSERVERIP:9005/