## Configuration of the AdLer Backend

## Konfigurationsdatei
In dem Verzeichnis `AdLerBackend.API/config` kann das AdLerBackend konfiguriert werden.
Dazu muss die Datei `config.json` angepasst bzw. erstellt werden.

Die Momentanen Konfigurationsmöglichkeiten sind:
- `useHttps` (Boolean) - Bestimmt, ob das Backend Https nutzen soll. Wenn `true` muss auch ein gültiges Zertifikat angegeben werden.
- `httpsCertificatePassword` (String) - Das Passwort für das Zertifikat, welches für Https verwendet wird.
- `httpPort` (Integer) - Der Port, welcher für Http verwendet wird.
- `httpsPort` (Integer) - Der Port, welcher für Https verwendet wird.


### Ein Https Zertifikat bereitstellen
Wenn `useHttps` auf `true` gesetzt wird, muss ein gültiges Zertifikat angegeben werden.
Dazu muss in dem Ordner `AdLerBackend.API/config/cert` ein Zertifikat abgelegt werden.
