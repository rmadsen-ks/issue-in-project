# Github Action to move issues in/into projects 

TBD

## Usage

Add a workflow like below to `.github/workflows/issue-in-project.yaml`:

```yaml
on:
  issues:
    types: [closed]

permissions:
  contents: read
  repository-projects: write
  issues: write

jobs:
  issue-in-project:
    runs-on: ubuntu-latest
    steps:
      - uses: AsgerIversen/issue-in-project
        with:
          token: ${{ secrets.GITHUB_TOKEN }}
          issue-closed-to-project: AsgerIversen/issue-in-project/1 
          issue-closed-to-column: start
```
or for better performance (not building the docker image each time):
```yaml
    steps:
      - uses: docker://ghcr.io/asgeriversen/issue-in-project:v1
        env:
          token: ${{ secrets.GITHUB_TOKEN }}
          issue-closed-to-project: AsgerIversen/issue-in-project/1 
          issue-closed-to-column: start
```

## Inputs:

### `token`
Gibhub access token used to manipulate the project. 

This can either be a personal access token or the GITHUB_TOKEN passed to the job by Github. The token needs write permissions to `repository-projects`. This permission can be granted to GITHUB_TOKEN using `permissions:` in the workflow YAML like in the example above.

### `issue-closed-to-project`

Set this to move all closed issues to a given project. The value should specify which project to move issues to in the format `<owner>/<repo>/<projectID>`.

### `issue-closed-to-column`

When using `issue-closed-to-project` this input controls which column in the project issues are added to.