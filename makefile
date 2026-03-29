VERSION := 0.0.1
REPO    := cpimhoff/ModSmith-Sts2
OUT     := release/
NUGET_KEY ?= $(shell cat ~/.nuget/apikey)

# macOS
MODSMITH_DIR := $(HOME)/Library/Application Support/Steam/steamapps/common/Slay the Spire 2/SlayTheSpire2.app/Contents/MacOS/mods

.PHONY: build buildcode release clean

build:
	dotnet publish

buildcode: # for code-only changes; significantly faster
	dotnet build

release: release-modsmith release-modtemplate release-docs

release-modsmith: clean build
	mkdir -p $(OUT)
	zip -j $(OUT)ModSmith.zip \
		"$(MODSMITH_DIR)/ModSmith/ModSmith.dll" \
		"$(MODSMITH_DIR)/ModSmith/ModSmith.json" \
		"$(MODSMITH_DIR)/ModSmith/ModSmith.pck"
	gh release create v$(VERSION) \
		$(OUT)ModSmith.zip \
		--repo $(REPO) \
		--title "v$(VERSION)"

release-modtemplate:
	mkdir -p $(OUT)
	dotnet pack ModTemplate/ModTemplate.Template.csproj -o $(OUT) -p:Version=$(VERSION)
	dotnet nuget push $(OUT)ModSmith.Templates.$(VERSION).nupkg \
			--api-key $(NUGET_KEY) \
			--source https://api.nuget.org/v3/index.json

release-docs:
	cd docsite && make release

clean:
	dotnet clean
	rm -rf $(OUT)
