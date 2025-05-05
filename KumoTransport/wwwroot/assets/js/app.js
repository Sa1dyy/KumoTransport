// === Globální proměnné ===
let scene, camera, renderer, model, mixer, trunkAnimation;
let isFocused = false;
let isTrunkOpen = false;
const clock = new THREE.Clock();
const defaultRotation = Math.PI / 0.55;
const ROTATIONS = {
  default: defaultRotation,
  door: (3 * Math.PI) / 2,
  trunk: Math.PI,
  hood: 0
};

function init() {
  scene = new THREE.Scene();

  camera = new THREE.PerspectiveCamera(50, window.innerWidth / window.innerHeight, 0.1, 1000);
  camera.position.set(0, 0.8, 5);

  renderer = new THREE.WebGLRenderer({ antialias: true, alpha: true });
  renderer.setSize(window.innerWidth, window.innerHeight);
  renderer.shadowMap.enabled = true;
  renderer.toneMapping = THREE.ReinhardToneMapping;
  renderer.toneMappingExposure = 0.9;

  document.getElementById("car-container").appendChild(renderer.domElement);

  const light = new THREE.DirectionalLight(0xffffff, 2);
  light.position.set(5, 5, 5);
  light.castShadow = true;
  scene.add(light);

  scene.add(new THREE.AmbientLight(0xffffff, 1.5));
  scene.add(new THREE.DirectionalLight(0xffffff, 0.8).position.set(-5, 3, -3));

  const plane = new THREE.Mesh(
    new THREE.PlaneGeometry(10, 10),
    new THREE.ShadowMaterial({ opacity: 0.3 })
  );
  plane.rotation.x = -Math.PI / 2;
  plane.position.y = -0.1;
  plane.receiveShadow = true;
  scene.add(plane);

  const loader = new THREE.GLTFLoader();
  loader.load("/assets/glb/van.glb", (gltf) => {
    model = gltf.scene;
    model.position.y = 0;
    model.rotation.y = defaultRotation;
    model.scale.set(0.7, 0.7, 0.7);

    model.traverse((node) => {
      if (node.isMesh) {
        node.castShadow = true;
        if (node.material && node.material.metalness !== undefined) {
          node.material.roughness = 0.4;
          node.material.metalness = 0.4;
          node.material.needsUpdate = true;
        }
      }
    });

    scene.add(model);

    if (gltf.animations.length > 0) {
      mixer = new THREE.AnimationMixer(model);
      const fullClip = gltf.animations[0];
      const start = fullClip.duration - 3;
      const end = fullClip.duration;
      const truncatedClip = THREE.AnimationUtils.subclip(fullClip, "openTrunkTrimmed", start * 30, end * 30);
      trunkAnimation = mixer.clipAction(truncatedClip);
      trunkAnimation.clampWhenFinished = true;
      trunkAnimation.loop = THREE.LoopOnce;
    }

    animate();
  });
}

function animate() {
  requestAnimationFrame(animate);
  const delta = clock.getDelta();
  if (mixer) mixer.update(delta);
  renderer.render(scene, camera);
}

function normalizeAngle(angle) {
  return ((angle % (2 * Math.PI)) + 2 * Math.PI) % (2 * Math.PI);
}

function moveToPart(targetRotation, titleText, position, clickedPart) {
  if (!model) return;

  const trunkRight = document.querySelector("#info-layout-trunk .trunk-right");
  const startRotation = model.rotation.y;
  const duration = 500;
  const startTime = performance.now();

  const delta = (() => {
    const normStart = normalizeAngle(startRotation);
    const normTarget = normalizeAngle(targetRotation);
    let d = normTarget - normStart;
    if (d > Math.PI) d -= 2 * Math.PI;
    if (d < -Math.PI) d += 2 * Math.PI;
    return d;
  })();

  function animateRotation(time) {
    const progress = Math.min((time - startTime) / duration, 1);
    model.rotation.y = startRotation + delta * progress;
    if (progress < 1) requestAnimationFrame(animateRotation);
    else finishFocusTransition();
  }

  function finishFocusTransition() {
    document.getElementById("info-box").style.visibility = "visible";
    isFocused = true;
    updateUI(titleText, position, clickedPart);
    document.getElementById("background-text").textContent = titleText;
    document.getElementById("back-button").classList.remove("hidden");

    if (clickedPart === "trunk" && trunkAnimation) {
      trunkAnimation.reset();
      trunkAnimation.play();
      isTrunkOpen = true;
      setTimeout(() => {
        if (trunkRight) trunkRight.classList.add("visible");
      }, 500);
    }
  }

  requestAnimationFrame(animateRotation);
}

function updateUI(text = "", position = "", clickedPart = "") {
  const body = document.body;
  const backgroundText = document.getElementById("background-text");
  const infoBox = document.getElementById("info-box");
  const infoTitle = document.getElementById("info-title");
  const clickablePoints = document.querySelectorAll(".clickable-point");
  const menuButton = document.getElementById("menu-button");

  document.getElementById("investor-button-wrapper").style.display = isFocused ? "none" : "block";

  document.querySelectorAll(".info-layout").forEach(el => el.classList.add("hidden"));
  document.querySelector("#info-layout-trunk .trunk-right")?.classList.remove("visible");
  infoBox.classList.remove("info-left", "info-right", "info-bottom", "info-top");
  document.getElementById("page-footer").style.display = isFocused ? "none" : "block";
  if (position) infoBox.classList.add(`info-${position}`);

  if (isFocused) {
    body.classList.replace("scene-default", "scene-focused");
    backgroundText.style.display = "none";
    clickablePoints.forEach(p => p.style.display = "none");
    infoBox.classList.remove("hidden");
    document.getElementById("site-logo").src = "/uploads/img/GroupLogoWhite.png";
    menuButton.classList.replace("default", "focused");

    if (clickedPart === "hood") infoTitle.style.display = "none";
    else {
      infoTitle.style.display = "block";
      infoTitle.textContent = text;
    }

    if (clickedPart === "door") {
      document.getElementById("info-layout-door").classList.remove("hidden");
      document.getElementById("info-main-text").textContent = kumoTexts.door;
      document.getElementById("info-title-left").textContent = text;
      infoTitle.style.display = "none";
    } else if (clickedPart === "trunk") {
      document.getElementById("info-layout-trunk").classList.remove("hidden");
      document.getElementById("info-text-trunk").textContent = kumoTexts.trunk;
      document.getElementById("trunk-title").textContent = text;
      infoTitle.style.display = "none";
    } else if (clickedPart === "hood") {
      document.getElementById("info-layout-hood").classList.remove("hidden");
    }
  } else {
    body.classList.replace("scene-focused", "scene-default");
    backgroundText.style.display = "block";
    backgroundText.textContent = "KUMO";
    clickablePoints.forEach(p => p.style.display = "block");
    infoBox.classList.add("hidden");
    document.getElementById("site-logo").src = "/uploads/img/GroupLogoBlack.png";
    menuButton.classList.replace("focused", "default");
  }
}

function resetView() {
  if (!model) return;

  const startRotation = model.rotation.y;
  const target = defaultRotation;
  const duration = 500;
  const startTime = performance.now();

  function animateRotation(time) {
    const progress = Math.min((time - startTime) / duration, 1);
    model.rotation.y = startRotation + (target - startRotation) * progress;
    if (progress < 1) requestAnimationFrame(animateRotation);
    else {
      isFocused = false;
      updateUI();
      document.getElementById("back-button").classList.add("hidden");
    }
  }

  if (trunkAnimation && isTrunkOpen) {
    document.querySelector("#info-layout-trunk .trunk-right")?.classList.remove("visible");
    trunkAnimation.stop();
    trunkAnimation.reset();
    isTrunkOpen = false;
  }

  requestAnimationFrame(animateRotation);
}

function toggleMenuButtons(show) {
  document.getElementById("menu-overlay").classList.toggle("hidden", !show);
  document.getElementById("menu-button").classList.toggle("hidden", show);
  document.getElementById("close-menu").classList.toggle("hidden", !show);
}

function transitionToPart(rotation, title, position, part) {
  document.getElementById("info-box").style.visibility = "hidden";
  document.querySelectorAll(".info-layout").forEach(layout => layout.classList.add("hidden"));
  document.querySelector("#info-layout-trunk .trunk-right")?.classList.remove("visible");
  if (trunkAnimation && isTrunkOpen) {
    trunkAnimation.stop();
    trunkAnimation.reset();
    isTrunkOpen = false;
  }
  toggleMenuButtons(false);
  moveToPart(rotation, title, position, part);
}

window.onload = () => {
  init();
  startPulsingPoints();

  document.getElementById("door-point").addEventListener("click", e => {
    transitionToPart(ROTATIONS.door, e.currentTarget.dataset.titleText, "bottom", "door");
  });
  document.getElementById("trunk-point").addEventListener("click", e => {
    transitionToPart(ROTATIONS.trunk, e.currentTarget.dataset.titleText, "left", "trunk");
  });
  document.getElementById("hood-point").addEventListener("click", e => {
    transitionToPart(ROTATIONS.hood, e.currentTarget.dataset.titleText, "left", "hood");
  });

  document.querySelectorAll(".menu-link").forEach(link => {
    link.addEventListener("click", () => {
      let rot = 0, title = link.dataset.title, position = "", part = "";
      switch (link.dataset.part) {
        case "about": rot = ROTATIONS.door; title = "ABOUT US"; position = "bottom"; part = "door"; break;
        case "services": rot = ROTATIONS.trunk; title = "SERVICES"; position = "left"; part = "trunk"; break;
        case "contact": rot = ROTATIONS.hood; title = "CONTACT"; position = "left"; part = "hood"; break;
        case "investors": rot = ROTATIONS.default; title = "PRO INVESTORY"; position = "right"; part = "investors"; break;
      }
      transitionToPart(rot, title, position, part);
    });
  });

  document.getElementById("menu-button").addEventListener("click", () => toggleMenuButtons(true));
  document.getElementById("close-menu").addEventListener("click", () => toggleMenuButtons(false));
  document.getElementById("back-button").addEventListener("click", resetView);
};

function createPulseEffect(pointElement) {
  const circle = document.createElement("div");
  circle.classList.add("pulse-circle");
  pointElement.appendChild(circle);
  setTimeout(() => pointElement.removeChild(circle), 1500);
}

function startPulsingPoints() {
  const points = document.querySelectorAll(".clickable-point");
  setInterval(() => {
    const randomPoint = points[Math.floor(Math.random() * points.length)];
    createPulseEffect(randomPoint);
  }, 2000);
}