# Selfhosted AzureDevOpos pipeline agents


## Environment Variables
Both linux and windows docker agents expects below Environment variables

Environment variables  | Description
  ------------- | -------------
AZP_URL  | The URL of the Azure DevOps or Azure DevOps Server instance
AZP_TOKEN  | Personal Access Token (PAT) granting access to AZP_URL
AZP_AGENT_NAME | Agent name (default value: the container hostname)
AZP_POOL | Agent pool name (default value: Default)
AZP_WORK | Work directory (default value: _work)

Start container
------------------
```docker run -e AZP_URL=<Azure DevOps instance> -e AZP_TOKEN=<PAT token> -e AZP_AGENT_NAME=mydockeragent dockeragent:latest```


Docker Images
------------------
https://hub.docker.com/r/sparekh1/azpipeagents

References
------------------
https://docs.microsoft.com/en-us/azure/devops/pipelines/agents/docker?view=azure-devops
