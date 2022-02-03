# Github Action to move issues in/into projects 

TBD

## Prerequisites

TBD

## Usage

To use get comments on merged PRs in your GitHub repository, add a workflow like below to `.github/workflows/issue-in-project.yaml`:

```
push:
    branches:
      - 'main'

permissions:
  contents: read
  pull-requests: write

jobs:
  issue-in-project:
    runs-on: ubuntu-latest
    name: issue-in-project
    steps:
      - name: Checkout
        uses: actions/checkout@v2
        with:
          # To use this action, you must check out the entire history of the repository
          fetch-depth: 0
      - name: Run comment action
        uses: AsgerIversen/issue-in-project
        with:
          token: ${{ secrets.GITHUB_TOKEN }}
          body: This change is part of version `{version}` or later.
```
