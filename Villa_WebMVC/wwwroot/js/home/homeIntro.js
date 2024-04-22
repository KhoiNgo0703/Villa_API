let canvas, ctx, w, h, trees;
let branchChance = [0.08, 0.09, 0.10, 0.11, 0.12, 0.15, 0.3];
let branchAngles = [20, 25, 30, 35];

function init() {
    canvas = document.querySelector("#canvas");
    ctx = canvas.getContext("2d");
    resizeReset();
    animationLoop();
}

function addTree(e) {
    trees.push(new Tree(e.x));
}

function resizeReset() {
    w = canvas.width = window.innerWidth;
    h = canvas.height = window.innerHeight;
    trees = [];
    drawGround();
    trees.push(new Tree());
}

function drawGround() {
    ctx.fillStyle = `rgba(34,42,18, 0.8)`;
    ctx.fillRect(0, h - 10, w, h);
}

function animationLoop() {
    drawScene();
    requestAnimationFrame(animationLoop);
}

function drawScene() {
    trees.map((t) => {
        t.update();
        t.draw();
    });
    drawText(); // Add this line to draw text in the middle
}

function drawText() {
    ctx.fillStyle = "black"; // Text color
    // First line of text
    ctx.font = "bold 80px  serif"; // Font size and family for Line 1
    ctx.textAlign = "center"; // Align text to center
    ctx.textBaseline = "middle"; // Center text vertically
    ctx.fillText("KoyFish Villa", w / 2, h / 2 - 20); // Text position for Line 1
    //// Second line of text
    //ctx.font = "30px serif"; // Font size and family for Line 2
    //ctx.fillText("Top 1 place for people who need healing", w / 2, h / 2 + 40); // Text position for Line 2
    
}

function getRandomInt(min, max) {
    return Math.round(Math.random() * (max - min)) + min;
}

class Tree {
    constructor(x) {
        this.x = (x) ? x : w * 0.5;
        this.y = h;
        this.branchs = [];
        this.addBranch(this.x, this.y, getRandomInt(5, 7), 180);
    }
    addBranch(x, y, radius, angle) {
        this.branchs.push(new Branch(x, y, radius, angle));
    }
    draw() {
        this.branchs.map((b) => {
            b.draw();
        });
    }
    update() {
        this.branchs.map((b) => {
            b.update();

            // Add branch when conditions are true
            if (b.radius > 0 && b.progress > 0.4 && Math.random() < b.branchChance && b.branchCount < 3) {
                let newBranch = {
                    x: b.x,
                    y: b.y,
                    radius: b.radius - 1,
                    angle: b.angle + branchAngles[Math.floor(Math.random() * branchAngles.length)] * b.branchDirection
                }
                this.addBranch(newBranch.x, newBranch.y, newBranch.radius, newBranch.angle);

                b.branchCount++;
                b.branchDirection *= -1;
            }
        });
    }
}

class Branch {
    constructor(x, y, radius, angle) {
        this.x = x;
        this.y = y;
        this.radius = radius;
        this.angle = angle;
        this.branchReset();
    }
    branchReset() {
        this.sx = this.x;
        this.sy = this.y;
        this.length = this.radius * 20;
        this.progress = 0;
        this.branchChance = branchChance[5 - this.radius];
        this.branchCount = 0;
        this.branchDirection = (Math.random() < 0.5) ? -1 : 1;
    }
    draw() {
        if (this.progress > 1 || this.radius <= 0) return;
        ctx.beginPath();
        ctx.arc(this.x, this.y, this.radius, 0, Math.PI * 2);
        ctx.fillStyle = `rgba(150,180,78, 0.8)`;
        ctx.fill();
        ctx.closePath();
    }
    update() {
        let radian = (Math.PI / 180) * this.angle;
        this.x = this.sx + (this.length * this.progress) * Math.sin(radian);
        this.y = this.sy + (this.length * this.progress) * Math.cos(radian);

        if (this.radius == 1) {
            this.progress += .05;
        } else {
            this.progress += .1 / this.radius;
        }

        if (this.progress > 1) {
            this.radius -= 1;
            this.angle += (Math.floor(Math.random() * 3) - 1) * 10;
            this.branchReset();
        }
    }
}

window.addEventListener("DOMContentLoaded", init);
window.addEventListener("resize", resizeReset);
window.addEventListener("click", addTree);
