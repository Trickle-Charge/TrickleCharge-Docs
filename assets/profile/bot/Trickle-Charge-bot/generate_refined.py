import os
from PIL import Image, ImageDraw

OUT_DIR = "/opt/data/avatar_designs"
SIZE = 512
GREEN = (0, 255, 0, 255)
DARK = (13, 17, 23, 255) # GitHub dark mode background

def draw_org_tc_brackets(draw, stroke=40):
    """Exact Trickle-Charge Org Brackets (Unconnected)"""
    # Left 'T' piece: 
    # Horizontal top bar: (100, 85) to (195, 85+stroke)
    # Vertical stem: (155, 85) to (195, 355)
    draw.rectangle([100, 85, 195, 85 + stroke], fill=GREEN)
    draw.rectangle([155, 85, 195, 355], fill=GREEN)
    
    # Right 'C' piece:
    # Top bar: (315, 75), Vertical: (315, 75) to (355, 425), Bottom bar: (315, 425-stroke) to (410, 425)
    draw.rectangle([315, 75, 410, 75 + stroke], fill=GREEN)
    draw.rectangle([315, 75, 355, 425], fill=GREEN)
    draw.rectangle([315, 425 - stroke, 410, 425], fill=GREEN)

def gen_v1_pure_floating_bot():
    """V1: Exact TC Org Brackets + Floating Bold Solid Robot Head"""
    img = Image.new("RGBA", (SIZE, SIZE), (0,0,0,0))
    draw = ImageDraw.Draw(img)
    draw_org_tc_brackets(draw, stroke=38)
    
    # Solid Bot Head in center (205 to 305, 185 to 295)
    draw.rectangle([210, 180, 300, 280], fill=GREEN)
    
    # Bold Cutout Eyes (High Contrast Dark)
    draw.rectangle([224, 205, 246, 235], fill=DARK)
    draw.rectangle([264, 205, 286, 235], fill=DARK)
    
    # Bright Pupils
    draw.rectangle([230, 213, 240, 227], fill=GREEN)
    draw.rectangle([270, 213, 280, 227], fill=GREEN)
    
    # Cutout Smile / Mouth
    draw.rectangle([228, 252, 282, 264], fill=DARK)
    
    # Bold Antenna
    draw.rectangle([248, 135, 262, 180], fill=GREEN)
    draw.rectangle([241, 120, 269, 135], fill=GREEN)
    
    return img

def gen_v2_charging_bot():
    """V2: Bot with Lightning / Trickle-Charge Bolt Antenna"""
    img = Image.new("RGBA", (SIZE, SIZE), (0,0,0,0))
    draw = ImageDraw.Draw(img)
    draw_org_tc_brackets(draw, stroke=38)
    
    # Head outline (heavy 16px stroke)
    draw.rectangle([208, 185, 302, 290], outline=GREEN, width=16)
    
    # Big bright glowing eye blocks
    draw.rectangle([224, 210, 248, 242], fill=GREEN)
    draw.rectangle([262, 210, 286, 242], fill=GREEN)
    
    # Mouth line
    draw.rectangle([226, 262, 284, 274], fill=GREEN)
    
    # Bolt Antenna
    # Zig-zag lightning bolt
    bolt = [
        (258, 115),
        (244, 150),
        (256, 150),
        (246, 185),
        (266, 145),
        (254, 145)
    ]
    draw.polygon(bolt, fill=GREEN)
    
    return img

def gen_v3_cradle_bold():
    """V3: Classic Cradle structure but Maximized Head & Scalable Features"""
    img = Image.new("RGBA", (SIZE, SIZE), (0,0,0,0))
    draw = ImageDraw.Draw(img)
    
    stroke = 38
    # Left T
    draw.rectangle([95, 80, 190, 80 + stroke], fill=GREEN)
    draw.rectangle([152, 80, 190, 370], fill=GREEN)
    
    # Right C
    draw.rectangle([320, 135, 415, 135 + stroke], fill=GREEN)
    draw.rectangle([320, 135, 358, 425], fill=GREEN)
    draw.rectangle([320, 425 - stroke, 415, 425], fill=GREEN)
    
    # Bottom cradle connection
    draw.rectangle([152, 370 - stroke, 320, 370], fill=GREEN)
    
    # Large Head Frame inside cradle
    draw.rectangle([200, 185, 310, 315], outline=GREEN, width=16)
    
    # Bold Eyes
    draw.rectangle([218, 215, 244, 252], fill=GREEN)
    draw.rectangle([266, 215, 292, 252], fill=GREEN)
    
    # Clean 3-bar mouth
    draw.rectangle([224, 275, 238, 295], fill=GREEN)
    draw.rectangle([248, 275, 262, 295], fill=GREEN)
    draw.rectangle([272, 275, 286, 295], fill=GREEN)
    
    # Antenna
    draw.rectangle([247, 135, 263, 185], fill=GREEN)
    draw.rectangle([238, 120, 272, 135], fill=GREEN)
    
    # Side ear bolts
    draw.rectangle([190, 232, 200, 268], fill=GREEN)
    draw.rectangle([310, 232, 320, 268], fill=GREEN)
    
    return img

def gen_v4_minimal_visor():
    """V4: Ultra Minimalist Cyber Visor (Highest legibility at 16px)"""
    img = Image.new("RGBA", (SIZE, SIZE), (0,0,0,0))
    draw = ImageDraw.Draw(img)
    
    draw_org_tc_brackets(draw, stroke=38)
    
    # Sleek cyber visor
    draw.rounded_rectangle([204, 190, 306, 280], radius=14, outline=GREEN, width=16)
    
    # Dual Visor Slits
    draw.rounded_rectangle([220, 222, 246, 252], radius=4, fill=GREEN)
    draw.rounded_rectangle([264, 222, 290, 252], radius=4, fill=GREEN)
    
    # Antenna
    draw.rectangle([248, 140, 262, 190], fill=GREEN)
    draw.ellipse([242, 122, 268, 148], fill=GREEN)
    
    return img

def build_refined_showcase():
    orig_raw = Image.open("/opt/data/avatar_temp/Trickle-Charge-bot.png").convert("RGBA").resize((SIZE, SIZE), Image.Resampling.LANCZOS)
    pog_raw = Image.open("/opt/data/avatar_temp/pog7776.png").convert("RGBA").resize((SIZE, SIZE), Image.Resampling.LANCZOS)
    tc_raw = Image.open("/opt/data/avatar_temp/Trickle-Charge.png").convert("RGBA").resize((SIZE, SIZE), Image.Resampling.LANCZOS)
    
    designs = [
        ("Current Bot (Faint)", orig_raw),
        ("A: Solid High-Contrast Bot", gen_v1_pure_floating_bot()),
        ("B: Lightning Charge Bot", gen_v2_charging_bot()),
        ("C: Heavy Cradle Classic", gen_v3_cradle_bold()),
        ("D: Cyber Visor Bot", gen_v4_minimal_visor()),
    ]
    
    # Save individual high-res PNGs
    for code_name, im in designs:
        fname = code_name.split(":")[0].replace(" ", "_").lower() + ".png"
        im.save(os.path.join(OUT_DIR, fname))
        
        # Also dark background variant
        bg = Image.new("RGBA", (SIZE, SIZE), DARK)
        bg.paste(im, (0, 0), im)
        bg.save(os.path.join(OUT_DIR, fname.replace(".png", "_dark.png")))
        
    # Build showcase board
    board = Image.new("RGBA", (1400, 840), DARK)
    d = ImageDraw.Draw(board)
    
    # Reference Header
    d.text((40, 20), "TRICKLE-CHARGE BRAND FAMILY", fill=(140, 140, 140))
    board.paste(pog_raw.resize((64, 64), Image.Resampling.LANCZOS), (40, 45), pog_raw.resize((64, 64)))
    d.text((115, 65), "pog7776 (Jack)", fill=(255, 255, 255))
    
    board.paste(tc_raw.resize((64, 64), Image.Resampling.LANCZOS), (300, 45), tc_raw.resize((64, 64)))
    d.text((375, 65), "Trickle-Charge Org (T-C)", fill=(255, 255, 255))
    
    d.line([(40, 125), (1360, 125)], fill=(35, 40, 50), width=2)
    
    col_w = 260
    start_x = 40
    
    for i, (title, img) in enumerate(designs):
        cx = start_x + (i * col_w)
        
        # Title
        d.text((cx, 140), title, fill=(100, 255, 100) if i > 0 else (180, 100, 100))
        
        # 1. 220px display
        v220 = img.resize((220, 220), Image.Resampling.LANCZOS)
        d.rounded_rectangle([cx, 170, cx + 220, 390], radius=8, outline=(35, 40, 50), width=1)
        board.paste(v220, (cx, 170), v220)
        
        # 2. Simulated 48px circle (GitHub Profile / Issue header)
        v48 = img.resize((48, 48), Image.Resampling.LANCZOS)
        mask48 = Image.new("L", (48, 48), 0)
        ImageDraw.Draw(mask48).ellipse([0, 0, 48, 48], fill=255)
        d.ellipse([cx + 10, 430, cx + 58, 478], fill=(22, 27, 34), outline=(48, 54, 61), width=1)
        board.paste(v48, (cx + 10, 430), mask48)
        d.text((cx + 70, 445), "48px Profile UI", fill=(170, 170, 170))
        
        # 3. Simulated 24px circle (GitHub Commits / Contributor list)
        v24 = img.resize((24, 24), Image.Resampling.LANCZOS)
        mask24 = Image.new("L", (24, 24), 0)
        ImageDraw.Draw(mask24).ellipse([0, 0, 24, 24], fill=255)
        d.ellipse([cx + 10, 520, cx + 34, 544], fill=(22, 27, 34), outline=(48, 54, 61), width=1)
        board.paste(v24, (cx + 10, 520), mask24)
        d.text((cx + 45, 525), "24px Contributor UI", fill=(170, 170, 170))
        
        # 4. Simulated 16px circle (Tiny favicon / commit avatar)
        v16 = img.resize((16, 16), Image.Resampling.LANCZOS)
        mask16 = Image.new("L", (16, 16), 0)
        ImageDraw.Draw(mask16).ellipse([0, 0, 16, 16], fill=255)
        d.ellipse([cx + 10, 590, cx + 26, 606], fill=(22, 27, 34), outline=(48, 54, 61), width=1)
        board.paste(v16, (cx + 10, 590), mask16)
        d.text((cx + 38, 592), "16px Micro UI", fill=(170, 170, 170))

    board_path = os.path.join(OUT_DIR, "bot_avatar_showcase.png")
    board.save(board_path)
    print("Showcase saved to", board_path)

if __name__ == "__main__":
    build_refined_showcase()
