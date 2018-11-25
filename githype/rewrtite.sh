#!/bin/sh

git filter-branch --env-filter '

OLD_EMAIL="ivan.antsipau@adform.com"
CORRECT_NAME="hypo-test"
CORRECT_EMAIL="ivan.antsipau2@gmail.com"

export GIT_COMMITTER_NAME="$CORRECT_NAME"
export GIT_COMMITTER_EMAIL="$CORRECT_EMAIL"

export GIT_AUTHOR_NAME="$CORRECT_NAME"
export GIT_AUTHOR_EMAIL="$CORRECT_EMAIL"
' --tag-name-filter cat -- --branches --tags