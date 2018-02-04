## Running on a Docker Host with Docker Compose
If you have a Docker host running or Docker locally installed on your machine, you don't even need the .NET Core runtime, SDK or Visual Studio 2017, nor an existing MongoDB host. Note that the included Compose files are meant for single Docker host, not a Swarm cluster.

Open a command line or shell window (i.e. PowerShell, Console, Bash etc.) and run the following commands:

### Windows 10
<code>   
$ cd \path\to\BookLibrary-NetCore<br />

$ docker-compose -f docker-compose.yml -f docker-compose.override.windows.yml up<br />
</code>

On Windows, you will need to use your machine's IP address on the `DockerNAT` virtual Ethernet adapter instead of `localhost`. You can get this by running `ipconfig` (it's usually `10.0.75.1`). Then open `http://DockerNAT-IP:5000`.

### Linux, macOS
<code>
$ cd /path/to/BookLibrary-NetCore<br />

$ docker-compose up<br />
</code>

Launch your web browser and open `http://localhost:5000`.