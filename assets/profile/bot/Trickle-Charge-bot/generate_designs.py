import os
import math
from PIL import Image, ImageDraw, ImageFont

OUT_DIR = "/opt/data/avatar_designs"
os.makedirs(OUT_DIR, exist_ok=True)

COLOR_GREEN = (0, 255, 0, 255)
COLOR_NEON = (0, 255, 0, 255) # Match exact pure green #00FF00
COLOR_DARK_BG = (13, 17, 23, 255) # GitHub dark mode background #0d1117
COLOR_TRANSPARENT = (0, 0, 0, 0)

SIZE = 512

def create_option_1_chunky_bot():
    """Option 1: Bold Minimalist Robot Head centered between TC brackets with thick strokes matching brackets."""
    img = Image.new("RGBA", (SIZE, SIZE), COLOR_TRANSPARENT)
    draw = ImageDraw.Draw(img)
    
    stroke = 42
    # Left T bracket: top bar (80, 70) to (180, 70+stroke), vertical stem (140, 70) to (180, 370)
    draw.rectangle([80, 70, 180, 70 + stroke], fill=COLOR_GREEN)
    draw.rectangle([138, 70, 180, 370], fill=COLOR_GREEN)
    
    # Right C bracket: top bar (330, 140) to (430, 140+stroke), vertical stem (330, 140) to (372, 440), bottom bar (330, 440-stroke) to (430, 440)
    draw.rectangle([330, 140, 430, 140 + stroke], fill=COLOR_GREEN)
    draw.rectangle([330, 140, 372, 440], fill=COLOR_GREEN)
    draw.rectangle([330, 440 - stroke, 430, 440], fill=COLOR_GREEN)
    
    # Bottom cradle bar connecting left and right
    draw.rectangle([138, 370 - stroke, 330, 370], fill=COLOR_GREEN)
    
    # Robot Head in the cradle:
    # Antenna
    draw.rectangle([244, 150, 268, 200], fill=COLOR_GREEN)
    draw.rectangle([232, 130, 280, 150], fill=COLOR_GREEN)
    
    # Bold head frame (outer 200 to 310, y 200 to 310, wall thickness 18px)
    draw.rectangle([200, 200, 310, 310], outline=COLOR_GREEN, width=18)
    
    # Solid chunky eyes (big rectangles: 26x30px)
    draw.rectangle([222, 226, 246, 256], fill=COLOR_GREEN)
    draw.rectangle([264, 226, 288, 256], fill=COLOR_GREEN)
    
    # Clean mouth bar / 3 wide teeth
    draw.rectangle([224, 276, 286, 292], fill=COLOR_GREEN)
    
    # Ears/side bolts (thick)
    draw.rectangle([186, 235, 200, 275], fill=COLOR_GREEN)
    draw.rectangle([310, 235, 324, 275], fill=COLOR_GREEN)
    
    return img

def create_option_2_cyber_visor_tc():
    """Option 2: High-contrast Cyber Visor Bot - The TC brackets cradle a sleek glowing bot visor with massive square eyes."""
    img = Image.new("RGBA", (SIZE, SIZE), COLOR_TRANSPARENT)
    draw = ImageDraw.Draw(img)
    
    stroke = 42
    # Left T
    draw.rectangle([75, 60, 185, 60 + stroke], fill=COLOR_GREEN)
    draw.rectangle([143, 60, 185, 380], fill=COLOR_GREEN)
    
    # Right C
    draw.rectangle([325, 130, 435, 130 + stroke], fill=COLOR_GREEN)
    draw.rectangle([325, 130, 367, 450], fill=COLOR_GREEN)
    draw.rectangle([325, 450 - stroke, 435, 450], fill=COLOR_GREEN)
    
    # Bottom cradle bar
    draw.rectangle([143, 380 - stroke, 325, 380], fill=COLOR_GREEN)
    
    # Bot Antenna (Signal emitter)
    draw.polygon([(255, 120), (240, 160), (270, 160)], fill=COLOR_GREEN)
    draw.rectangle([250, 160, 260, 195], fill=COLOR_GREEN)
    
    # Bot Visor (Wide, bold, instant readability)
    draw.rounded_rectangle([195, 195, 315, 325], radius=12, outline=COLOR_GREEN, width=20)
    
    # Glowing visor screen / dual high-visibility blocks
    draw.rounded_rectangle([218, 225, 248, 265], radius=4, fill=COLOR_GREEN)
    draw.rounded_rectangle([262, 225, 292, 265], radius=4, fill=COLOR_GREEN)
    
    # Equalizer / power charge meter mouth (4 vertical ticks)
    for bx in [224, 242, 260, 278]:
        draw.rectangle([bx, 285, bx + 8, 305], fill=COLOR_GREEN)
        
    return img

def create_option_3_integrated_bot_face():
    """Option 3: Pure Geometric Bot Face - The TC bracket silhouette seamlessly incorporates the robot character."""
    img = Image.new("RGBA", (SIZE, SIZE), COLOR_TRANSPARENT)
    draw = ImageDraw.Draw(img)
    
    stroke = 44
    # Left T
    draw.rectangle([80, 60, 190, 60 + stroke], fill=COLOR_GREEN)
    draw.rectangle([146, 60, 190, 390], fill=COLOR_GREEN)
    
    # Right C
    draw.rectangle([320, 120, 430, 120 + stroke], fill=COLOR_GREEN)
    draw.rectangle([320, 120, 364, 450], fill=COLOR_GREEN)
    draw.rectangle([320, 450 - stroke, 430, 450], fill=COLOR_GREEN)
    
    # Bottom cradle bar
    draw.rectangle([146, 390 - stroke, 320, 390], fill=COLOR_GREEN)
    
    # Large solid robot head block that fills the negative space with high contrast negative-cut features!
    head_box = [198, 175, 312, 335]
    # Draw filled solid green head
    draw.rectangle(head_box, fill=COLOR_GREEN)
    
    # Punch out negative space (transparent) for eyes and mouth so it pops on dark mode!
    # Cutout eyes
    draw.rectangle([216, 215, 246, 255], fill=COLOR_DARK_BG)
    draw.rectangle([264, 215, 294, 255], fill=COLOR_DARK_BG)
    
    # Inside each eye cutout, put a bright green cyber pupil / reticle!
    draw.rectangle([226, 225, 236, 245], fill=COLOR_GREEN)
    draw.rectangle([274, 225, 284, 245], fill=COLOR_GREEN)
    
    # Cutout mouth grill
    draw.rectangle([218, 285, 292, 305], fill=COLOR_DARK_BG)
    
    # Top antenna bulb
    draw.rectangle([245, 125, 265, 175], fill=COLOR_GREEN)
    draw.rectangle([235, 105, 275, 125], fill=COLOR_GREEN)
    
    return img

def create_option_4_retro_arcade_bot():
    """Option 4: Authentic Retro Arcade Pixel Bot (8-bit grid optimized for 16px/32px avatar rendering)."""
    img = Image.new("RGBA", (SIZE, SIZE), COLOR_TRANSPARENT)
    draw = ImageDraw.Draw(img)
    
    u = 32 # 16x16 grid on 512x512 canvas
    
    def rect(x1, y1, x2, y2, c=COLOR_GREEN):
        draw.rectangle([x1 * u, y1 * u, x2 * u, y2 * u], fill=c)
        
    # TC Brackets on 16-unit grid:
    # Left T (cols 2-5, rows 2-12)
    rect(2, 2, 5, 3)
    rect(4, 2, 5, 12)
    
    # Right C (cols 10-13, rows 4-14)
    rect(10, 4, 13, 5)
    rect(10, 4, 11, 14)
    rect(10, 13, 13, 14)
    
    # Bottom cradle
    rect(4, 11, 10, 12)
    
    # Robot Head (cols 6-9, rows 5-10)
    # Antenna
    rect(7, 4, 8, 4)
    rect(7, 5, 8, 6)
    
    # Head outline (rows 6 to 10)
    rect(6, 6, 9, 10)
    
    # Negative eye cutouts
    rect(6.5, 7, 7.5, 8.2, COLOR_DARK_BG)
    rect(8, 7, 9, 8.2, COLOR_DARK_BG)
    
    # Negative mouth cutout
    rect(6.8, 9, 8.7, 9.6, COLOR_DARK_BG)
    
    return img

def render_comparison_sheet():
    # Load original avatar and resize to 512x512
    orig_raw = Image.open("/opt/data/avatar_temp/Trickle-Charge-bot.png").convert("RGBA")
    orig = orig_raw.resize((SIZE, SIZE), Image.Resampling.LANCZOS)
    
    pog_raw = Image.open("/opt/data/avatar_temp/pog7776.png").convert("RGBA").resize((SIZE, SIZE), Image.Resampling.LANCZOS)
    tc_raw = Image.open("/opt/data/avatar_temp/Trickle-Charge.png").convert("RGBA").resize((SIZE, SIZE), Image.Resampling.LANCZOS)
    
    options = [
        ("Current Bot", orig),
        ("Option 1: Bold Classic", create_option_1_chunky_bot()),
        ("Option 2: Cyber Visor", create_option_2_cyber_visor_tc()),
        ("Option 3: High-Vis Negative Face", create_option_3_integrated_bot_face()),
    ]
    
    # Save standalone transparent and dark-bg versions
    for name, im in options:
        safe_name = name.lower().replace(" ", "_").replace(":", "").replace("-", "_")
        im.save(os.path.join(OUT_DIR, f"{safe_name}.png"))
        
        dark = Image.new("RGBA", (SIZE, SIZE), COLOR_DARK_BG)
        dark.paste(im, (0, 0), im)
        dark.save(os.path.join(OUT_DIR, f"{safe_name}_dark.png"))
        
    # Also save the reference trio
    pog_raw.save(os.path.join(OUT_DIR, "ref_pog7776.png"))
    tc_raw.save(os.path.join(OUT_DIR, "ref_trickle_charge_org.png"))

    # Build side-by-side presentation board
    # 1300x850 dark sheet
    board = Image.new("RGBA", (1360, 800), COLOR_DARK_BG)
    d = ImageDraw.Draw(board)
    
    # Top header: show pog7776 & Trickle-Charge org reference icons
    d.text((40, 25), "REFERENCE BRAND ICONS", fill=(160, 160, 160))
    board.paste(pog_raw.resize((80, 80), Image.Resampling.LANCZOS), (40, 50), pog_raw.resize((80, 80)))
    d.text((130, 75), "pog7776 (Jack)", fill=(255, 255, 255))
    
    board.paste(tc_raw.resize((80, 80), Image.Resampling.LANCZOS), (320, 50), tc_raw.resize((80, 80)))
    d.text((410, 75), "Trickle-Charge Org (T-C brackets)", fill=(255, 255, 255))
    
    d.line([(40, 145), (1320, 145)], fill=(40, 45, 55), width=2)
    
    # Row labels
    d.text((40, 160), "512px Master Render", fill=(140, 140, 140))
    d.text((40, 480), "GitHub 48px Avatar Preview", fill=(140, 140, 140))
    d.text((40, 620), "GitHub 24px Commit Preview", fill=(140, 140, 140))
    
    col_w = 320
    start_x = 40
    
    for i, (label, img) in enumerate(options):
        col_x = start_x + (i * col_w)
        
        # Column title
        d.text((col_x, 185), label, fill=(200, 255, 200) if i > 0 else (180, 180, 180))
        
        # 1. 240px main view
        v240 = img.resize((240, 240), Image.Resampling.LANCZOS)
        # draw a subtle container box
        d.rounded_rectangle([col_x, 215, col_x + 240, 455], radius=8, outline=(35, 40, 50), width=1)
        board.paste(v240, (col_x, 215), v240)
        
        # 2. 48px circular avatar (GitHub profile / issue commenter)
        v48 = img.resize((48, 48), Image.Resampling.LANCZOS)
        mask48 = Image.new("L", (48, 48), 0)
        ImageDraw.Draw(mask48).ellipse([0, 0, 48, 48], fill=255)
        
        # circular background circle
        d.ellipse([col_x + 20, 510, col_x + 68, 558], fill=(22, 27, 34), outline=(48, 54, 61), width=1)
        board.paste(v48, (col_x + 20, 510), mask48)
        d.text((col_x + 80, 525), "48px UI scale", fill=(180, 180, 180))
        
        # 3. 24px circular avatar (GitHub commit log / contributor list)
        v24 = img.resize((24, 24), Image.Resampling.LANCZOS)
        mask24 = Image.new("L", (24, 24), 0)
        ImageDraw.Draw(mask24).ellipse([0, 0, 24, 24], fill=255)
        
        d.ellipse([col_x + 20, 650, col_x + 44, 674], fill=(22, 27, 34), outline=(48, 54, 61), width=1)
        board.paste(v24, (col_x + 20, 650), mask24)
        d.text((col_x + 60, 654), "24px Contributor scale", fill=(180, 180, 180))
        
    board.save(os.path.join(OUT_DIR, "comparison_board.png"))
    print("Generated comparison board at /opt/data/avatar_designs/comparison_board.png")

if __name__ == "__main__":
    render_comparison_sheet()
