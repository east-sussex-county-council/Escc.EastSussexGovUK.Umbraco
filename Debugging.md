# Auditing and debugging

## Identifying the machine in a load-balanced environment

In a load-balanced environment sometimes there is a problem which affects one or more of the load-balanced machines, but not all. In this situation it can be useful to identify the machine which served a given response. For example, you can use this to identify the correct Umbraco log file in the `~\App_Data\Logs\` folder. 

`MachineNameModule` makes this possible by adding an `X-ESCC-Machine` HTTP header which can be viewed as part of the response in the developer tools of any web browser. This requires `MachineNameModule` to be configured in `web.config`:

	<system.webServer>
		<modules>
	      <remove name="MachineNameModule" />
	      <add name="MachineNameModule" type="Escc.EastSussexGovUK.Umbraco.AzureConfiguration.MachineNameModule, Escc.EastSussexGovUK.Umbraco" />
	    </modules>
	</system.webServer>

As the machine name is potentially sensitive information it includes two safeguards:

*  Only the last three characters of the machine name are returned. The actual machine name is therefore kept private, but the last three characters are enough to identify the actual machine if you already have access to the actual machine names (in the Umbraco log files for example).
*  The header is only returned for requests from a comma-separated whitelist of IP addresses, which is configured in `web.config`:

		<appSettings>
			<add key="IpAddressesForDebugInfo" value="1.2.3.4,5.6.7.8" />
		</appSettings>

## Logging who did what in Umbraco

`AuditLoggingEventHandler` adds information in the standard Umbraco log in `~\App_Data\Logs` whenever content and media are created, published or unpublished, copied, moved or deleted. It inherits from `ApplicationEventHandler` which means it is automatically activated.

This can be useful when tracking down what happened to a particular item that's either been misplaced or that triggered an error message which led to a bug report, or to confirm whether an action has happened when the UI is unresponsive.