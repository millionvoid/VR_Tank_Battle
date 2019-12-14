import json


def to_json(frame):
	dic = {}
	dic['fps']=frame.current_frames_per_second
	dic['timestamp']=frame.timestamp
	dic['is_valid']=frame.is_valid
	dic['id']=frame.id
	dic['hand_count']=len(frame.hands)
	if len(frame.hands)>0:
		#add leftmost
		leftmost_hand = frame.hands.leftmost
		leftmost_hand_dic = {}
		leftmost_hand_dic["is_left"]=leftmost_hand.is_left
		leftmost_hand_dic["is_right"]=leftmost_hand.is_right
		leftmost_hand_dic["is_valid"]=leftmost_hand.is_valid
		leftmost_hand_dic["pinch_strength"]=leftmost_hand.pinch_strength
		leftmost_hand_dic["grab_strength"]=leftmost_hand.grab_strength
		leftmost_hand_dic["palm_normal"]=leftmost_hand.palm_normal.to_float_array()
		leftmost_hand_dic["palm_position"]=leftmost_hand.palm_position.to_float_array()
		leftmost_hand_dic["palm_velocity"]=leftmost_hand.palm_velocity.to_float_array()
		leftmost_hand_dic["wrist_position"]=leftmost_hand.wrist_position.to_float_array()
		leftmost_hand_dic["sphere_center"]=leftmost_hand.sphere_center.to_float_array()
		leftmost_hand_dic["sphere_radius"]=leftmost_hand.sphere_radius
		fingers=[]
		extended_fingers = leftmost_hand.fingers.extended()
		for finger in leftmost_hand.fingers:
			finger_dic={}
			finger_dic["type"]=finger.type
			finger_dic["is_extended"]=finger in extended_fingers
			bones = []
			for bone_index in range(0,4):
				bone = finger.bone(bone_index)
				if bone.is_valid:
					bones.append((bone.prev_joint.to_float_array(),bone.next_joint.to_float_array()))
			finger_dic["bones"] = bones
			fingers.append(finger_dic)
		leftmost_hand_dic["fingers"]=fingers
		dic['leftmost_hand']=leftmost_hand_dic
	if len(frame.hands)>1:
		#add rightmost
		rightmost_hand = frame.hands.rightmost
		rightmost_hand_dic = {}
		rightmost_hand_dic["is_left"]=rightmost_hand.is_left
		rightmost_hand_dic["is_right"]=rightmost_hand.is_right
		rightmost_hand_dic["is_valid"]=rightmost_hand.is_valid
		rightmost_hand_dic["pinch_strength"]=rightmost_hand.pinch_strength
		rightmost_hand_dic["grab_strength"]=rightmost_hand.grab_strength
		rightmost_hand_dic["palm_normal"]=rightmost_hand.palm_normal.to_float_array()
		rightmost_hand_dic["palm_position"]=rightmost_hand.palm_position.to_float_array()
		rightmost_hand_dic["palm_velocity"]=rightmost_hand.palm_velocity.to_float_array()
		rightmost_hand_dic["wrist_position"]=rightmost_hand.wrist_position.to_float_array()
		rightmost_hand_dic["sphere_center"]=rightmost_hand.sphere_center.to_float_array()
		rightmost_hand_dic["sphere_radius"]=rightmost_hand.sphere_radius
		fingers=[]
		extended_fingers = rightmost_hand.fingers.extended()
		for finger in rightmost_hand.fingers:
			finger_dic={}
			finger_dic["type"]=finger.type
			finger_dic["is_extended"]=finger in extended_fingers
			bones = []
			for bone_index in range(0,4):
				bone = finger.bone(bone_index)
				if bone.is_valid:
					bones.append((bone.prev_joint.to_float_array(),bone.next_joint.to_float_array()))
			finger_dic["bones"] = bones
			fingers.append(finger_dic)
		rightmost_hand_dic["fingers"]=fingers
		dic['rightmost_hand']=rightmost_hand_dic
	return json.dumps(dic, indent=4) #nice to read
	# return json.dumps(dic) #hard to read


if __name__ == '__main__':
	pass