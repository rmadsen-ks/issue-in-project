# Github Action to move issues in/into projects 

TBD

## Prerequisites

TBD

## Usage

To use get comments on merged PRs in your GitHub repository, add a workflow like below to `.github/workflows/issue-in-project.yaml`:

```
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
    name: issue-in-project
    steps:
      - name: Run comment action
        uses: AsgerIversen/issue-in-project
        with:
          token: ${{ secrets.GITHUB_TOKEN }}
          issue-closed-to-project: AsgerIversen/issue-in-project/1 
          issue-closed-to-column: start
```

## Inputs:

| 