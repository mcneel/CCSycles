Prerequistes
============

Git (on the PATH)
Svn (on the PATH) (Andy used https://sliksvn.com/download/)

python (on the PATH) - type "python" - it needs to work)

	Andy used:
	https://www.python.org/ftp/python/3.11.3/python-3.11.3-amd64.exe
	Make sure you check "Add python.exe to PATH"

Powershell script executation rights (PS -> Set-ExecutionPolicy unrestricted -Scope CurrentUser)


Building
========

Ensure the development branch (jesterKing/8.x/manual_integration_cyclesx) is checked out, submodules updated:

From Powershell:

1. The first time, run cmd cycles\make update
2. run cycles\build_cycles_for_rhino.ps1  (Make sure you're in Powershell, not CMD!)


Merge workflow
=====================

There are feature branches for CyclesX manual integration in rhino.git, ccsycles.git, cycles.git and rhinocycles.git

Work in a branch called "localwork"

git fetch
git rebase origin/jesterKing/8.x/manual_integration_cyclesx
git checkout jesterKing/8.x/manual_integration_cyclesx
git pull --ff-only
git merge localwork
git push


