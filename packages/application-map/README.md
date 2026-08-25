# Local ApplicationMap packages

This directory temporarily makes the showcase independent from a local TinyObservability checkout
for package restore.

It contains only the unpublished ApplicationMap release train used by the sample:

- `TinyObservability.ApplicationMap.0.1.0-alpha.1-local.1.nupkg`
- `TinyObservability.ApplicationMap.TinyDispatcher.0.1.0-alpha.1-local.1.nupkg`
- `TinyObservability.ApplicationMap.TinyValidations.0.1.0-alpha.1-local.1.nupkg`

TinyDispatcher and TinyValidations are restored from NuGet.org. Do not add producer packages to this
directory.

These binaries must be removed when the matching ApplicationMap packages become available from a
public package source.

## SHA-256

```text
AE43B10FF326950870DD40DB66F0482ED22D8B3A9BE77568E53F8B83DA653645  TinyObservability.ApplicationMap.0.1.0-alpha.1-local.1.nupkg
2CF45DF553E0022DD06F92ED1B1D9C8E8976B405B912116AAAC9234DDF52BF25  TinyObservability.ApplicationMap.TinyDispatcher.0.1.0-alpha.1-local.1.nupkg
B3A4392DA69213EFD07D280AE4AA9C0919766B0AC4EC6B1E4AC7A39DC7562DEB  TinyObservability.ApplicationMap.TinyValidations.0.1.0-alpha.1-local.1.nupkg
```
