REPO := git@github.com:cpimhoff/ModSmith-Sts2.git
SITE_DIR := _site

.PHONY: build serve release clean

build:
	docfx docfx.json

serve:
	docfx docfx.json --serve

release: clean build
	cd $(SITE_DIR) && \
		git init && \
		git checkout -b gh-pages && \
		git add -A && \
		git commit -m "Deploy docs" && \
		git push -f $(REPO) gh-pages

clean:
	rm -rf $(SITE_DIR)
