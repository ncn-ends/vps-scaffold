## Introduction

This project is meant to scaffold an Ubuntu machine with the standard requirements to deploy a web service. This includes:
- Installing necessary packages
- Setting up a user with sudo privileges
- Basic security configurations
- Configuring SSH
- Configuring SSL
- Configuring firewalls
- Setting up Nginx, including Nginx server blocks

## Usage

1) Provision an Ubuntu VPS
2) Download the release package and place it to the remote server.
  - You can do this in a variety of ways, such as wget the release package, build the binary and sftp it, etc.
3) Making sure you're on root, run the command (with optional flags)
```
./vps-scaffold [--minimal]
               [--no-http / no-https]
               [--no-domain]
```
3) Follow the prompts and input them accordingly.

### Optional Parameters
#### `--minimal`
- User input prompts will be avoided unless crucial during set up process.

#### `--no-http` / `no-https`
- Redirects http requests to the described protocol at the nginx level.
- By default, both http and https requests will be accepted. 

#### `--no-domain`
- Will not ask the user for a domain name prompt and Nginx will be configued with the server's IP as the domain name for the server block.


## Plans

### Features

- Allow for multiple domain per server block configuration.

## Authors

- [@ncn-ends](https://www.github.com/ncn-ends)


## License

[MIT](https://choosealicense.com/licenses/mit/)