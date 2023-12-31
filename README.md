<p align="center">
	<img src="https://github-production-user-asset-6210df.s3.amazonaws.com/86578963/282287204-da933ad8-664b-429a-a2c9-ebf201a75707.png">
</p>
<hr>

[The Stanley Parable: Source 2](https://tspsource2.softsplit.org) is an ambitious project currently being developed by Softsplit Studios for S&box. The project's goal is to recreate the unique experience and mechanics of The Stanley Parable on Source 2, making full use of the engine's updated graphics and capabilities.

## Installation

This section provides guidance for developers on how to install and set up the TSPS2 addon for local development. For regular users and players, it is recommended to use S&box's in-game addon browser to play the gamemode.

- Navigate to your S&box installation directory. On Windows, this would typically be located at `Steam/steamapps/common/sbox/`.
- Inside the `addons` folder, clone the repository.
- Run the developer version of S&box (sbox-dev.exe). Once loaded, add the addon using the Project context menu (Project -> Add Existing From Disk). Confirm that it's active and enabled by hovering over the checkmark icon.

Congratulations! Your local version of TSPS2 is now ready for both playing and modification. If you encounter any issues during installation or setup, feel free to reach out to us on our [Discord](https://discord.gg/GaGFHFttAC).

## Launching & Updating

A convenient way to launch the game mode is by using [launch configurations](https://github-production-user-asset-6210df.s3.amazonaws.com/86578963/278296336-5e067d30-0808-4112-8fa4-1972ec596145.png). Alternatively, you can launch the gamemode using the in-game gamemode browser or through the console. If you prefer using the console, execute the following commands:
```
gamemode local.thestanleyparables2
map mapname
```

Updating the gamemode is simple; just pull the latest changes from the remote repository. Most of the time, the game will automatically recognize and apply these changes through hotloading. However, if you're performing major git operations, such as switching branches or merging, it's advisable to close the game before doing so. Please note that we gitignore most ``*_c`` files, so the gamemode will need to compile the first time you launch it.

# Contributing

Interested in contributing by fixing a bug or implementing a new feature? Whether it's a substantial change or a minor bugfix, we welcome your contributions through a pull request. Before you submit your changes, make sure to follow these guidelines:
- [The Pull Request Guidelines](https://github.com/Softsplit/TSP-Source-2/blob/main/.github/CONTRIBUTING.md)
- [The Pull Request Template](https://github.com/Softsplit/TSP-Source-2/blob/main/.github/PULL_REQUEST_TEMPLATE.md)

As long as you adhere to these guidelines, you can submit a pull request, and a member of the Softsplit development team will aim to review it promptly. 
Make sure to join our [Discord](https://discord.gg/GaGFHFttAC) to chat with other developers and share your ideas in dedicated channels!

# License & Disclaimer

This repository is covered by the [MIT license](https://github.com/Softsplit/TSP-Source-2/blob/main/LICENSE.md).

For more information on third-party licenses, please refer to the [Third-Party Licenses](https://github.com/Softsplit/TSP-Source-2/blob/main/THIRDPARTYLICENSES.md).
