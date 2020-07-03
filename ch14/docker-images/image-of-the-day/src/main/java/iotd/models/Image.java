package iotd;

public class Image {

    private String url;
    private String caption;
    private String copyright;

    public Image() {}

    public Image(String url, String caption, String copyright) {
        setUrl(url);
        setCaption(caption);
        setCopyright(copyright);
    }

    public String getUrl() {
    	return url;
    }
    
    public void setUrl(String url) {
        //TODO - if it's a YouTube link need to format the URL:
        // https://img.youtube.com/vi/<insert-youtube-video-id-here>/0.jpg
        this.url = url;
    }

    public String getCaption() {
    	return caption;
    }
    
    public void setCaption(String caption) {
        this.caption = caption;
    }

    public String getCopyright() {
    	return copyright;
    }
    
    public void setCopyright(String copyright) {
        this.copyright = copyright;
    }

}
