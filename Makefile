SUBDIRS:= Compatibility-v13/bindings Compatibility-v4/bindings GoogleMaps/bindings

SUBDIRS_MAKE= @target=`echo $@ | sed -e 's/-recurse//'` && \
               for dir in $(SUBDIRS); do        \
                echo "Making $$target in $$dir";\
                $(MAKE) -C $$dir $$target || exit 1; \
               done

all:
	$(SUBDIRS_MAKE)