## Introduction



## Usage

1) Download the release package.
2) Run the command
3) Follow the prompts and input them accordingly.

### Assumptions

- The program assumes that it will be run on root initially.
- SSL/TLS is not configured
- The program does not assign a domain to the Nginx configuration and instead uses the public IPv4 address assigned to the server. 
  - Note: You can just assign a domain to the IP and get it working with http requests, but note that some domains require https when requesting from browsers. See HSTS domains list.

## Plans

### Features

- Disable root login
- Configure "minimal" mode, which doesn't have as many prompts to guide the user.
- Allow for SSL/TLS configuration
- Allow for proper domain configuration
- Allow for other types of deployments, such as ASP.NET, Python, etc.

### Structure and Organization

- Refactor all Pastel calls as custom extension methods corresponding to output type. 
  - Maybe use enums here for organization
