## Introduction

This project is meant to scaffold an Ubuntu installation with the necessary steps for deployment of a web service. This includes:
- Setting up a user with sudo privileges
- Basic security configurations
- Configuring SSH
- Configuring firewalls
- Setting up Nginx, including Nginx server blocks

## Usage

1) Download the release package and place it to the remote server.
2) Run the command (with optional flags)
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
- Will not ask the user for a domain name prompt.
- Nginx will be configued with the server's IP as the domain name for the server block.

### Assumptions

- The program assumes that it will be run on root initially.
- The program does not assign a domain to the Nginx configuration and instead uses the public IPv4 address assigned to the server. 
  - Note: You can just assign a domain to the IP and get it working with http requests, but note that some domains require https when requesting from browsers. See HSTS domains list.

## Plans

### Features

- Disable root login
- Allow for multiple domain per server block configuration.